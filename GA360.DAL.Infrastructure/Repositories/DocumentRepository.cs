using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GA360.DAL.Infrastructure.Repositories;

public class DocumentRepository : Repository<Document>, IDocumentRepository
{
    public DocumentRepository(CRMDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CleanOrphans(int userId, List<Document> documents)
    {
        try
        {
            var filteredDocuments = GetDbContext().Documents
          .Include(x => x.DocumentCustomers)
          .AsNoTrackingWithIdentityResolution()
          .ToList() // Switch to client-side evaluation
          .Where(x => x.DocumentCustomers.Any(dc => dc.CustomerId == userId)
                      && !documents.Any(d => d.BlobId == x.BlobId)
                      && x.BlobId.StartsWith($"{userId}/"))
          .ToList();

            GetDbContext().Documents.RemoveRange(filteredDocuments);

            await GetDbContext().SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {

            throw;
        }
       
        return false;
    }

    public async Task UpsertDocuments(int userId, List<Document> documents)
    {
        try
        {
            var existingEntities = GetDbContext().DocumentCustomer.Where(x => x.CustomerId == userId).ToList();

            if (documents != null)
            {
                var documentIds = GetDbContext().Documents
                    .Where(d => documents.Select(doc => doc.BlobId).Contains(d.BlobId))
                    .Select(d => d.Id)
                    .ToList();

                if (documentIds?.Count != 0)
                {
                    var documentIdsString = string.Join(",", documentIds);

                    var deleteDocumentCustomerSql = $"DELETE DC FROM DocumentCustomer DC " +
                                                    $"WHERE DC.CustomerId = @userId AND DC.DocumentId NOT IN ({documentIdsString})";

                    GetDbContext().Database.ExecuteSqlRaw(deleteDocumentCustomerSql,
            new SqlParameter("@userId", userId));
                }

                var documentsToUpload = new List<Document>();
                foreach (var document in documents)
                {
                    if (document.FileSize != "0")
                    {
                        var result = await GetDbContext().Documents.FirstOrDefaultAsync(x => x.BlobId.ToLower() == document.BlobId.ToLower());

                        if (result == null)
                        {
                            documentsToUpload.Add(new Document
                            {
                                BlobId = document.BlobId,
                                Path = document.Path,
                                Title = document.Title,
                                FileSize = document.FileSize,
                                Description = document.Title,
                                FileType = string.Empty,
                                Category = string.Empty
                            });
                        }
                    }
                }

                GetDbContext().Documents.AddRange(documentsToUpload);

                await GetDbContext().SaveChangesAsync();

                foreach (var toupload in documentsToUpload)
                {
                    GetDbContext().DocumentCustomer.Add(new DocumentCustomer
                    {
                        CustomerId = userId,
                        DocumentId = toupload.Id
                    });
                }

                await GetDbContext().SaveChangesAsync();

            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;

        }
    }
}
