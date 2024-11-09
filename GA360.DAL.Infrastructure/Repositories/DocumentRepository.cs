using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GA360.DAL.Infrastructure.Repositories;

public class DocumentRepository : Repository<Document>, IDocumentRepository
{
    private CRMDbContext _context;
    private ILogger<DocumentRepository> _logger;
    public DocumentRepository(CRMDbContext dbContext, ILogger<DocumentRepository> logger) : base(dbContext)
    {
        _context = dbContext;
        _logger = logger;
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
            // Retrieve existing DocumentCustomer entries for the user
            var documentsCustomers = await _context.DocumentCustomer
                .Include(x => x.Document)
                .Where(x => x.CustomerId == userId)
                .ToListAsync();

            // Get document IDs for existing entries
            var documentIds = documentsCustomers.Select(x => x.Document.Id).ToList();

            // Generate SQL to delete DocumentCustomer entries no longer in the list
            if (documentIds.Any())
            {
                var documentIdsString = string.Join(",", documentIds);
                var deleteDocumentCustomerSql = $"DELETE FROM DocumentCustomer " +
                                                $"WHERE CustomerId = @userId AND DocumentId NOT IN ({documentIdsString})";

                _context.Database.ExecuteSqlRaw(deleteDocumentCustomerSql, new SqlParameter("@userId", userId));
            }

            foreach (var document in documents)
            {
                var existingDocumentCustomer = documentsCustomers
                    .FirstOrDefault(x => x.Document.BlobId.ToLower() == document.BlobId.ToLower());

                if (existingDocumentCustomer == null)
                {
                    // Insert new document and DocumentCustomer entry
                    var newDocument = new Document
                    {
                        BlobId = document.BlobId,
                        Path = document.Path,
                        Title = document.Title,
                        FileSize = document.FileSize,
                        Description = document.Title,
                        FileType = string.Empty,
                        Category = string.Empty
                    };

                    _context.Documents.Add(newDocument);
                    await _context.SaveChangesAsync();

                    _context.DocumentCustomer.Add(new DocumentCustomer
                    {
                        CustomerId = userId,
                        DocumentId = newDocument.Id,
                    });

                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Update existing document
                    var existingDocument = existingDocumentCustomer.Document;
                    existingDocument.Title = document.Title;
                    existingDocument.Description = document.Title;

                    _context.Documents.Update(existingDocument);
                    await _context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while upserting documents.");
            throw;
        }
    }
}
