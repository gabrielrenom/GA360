using GA360.DAL.Entities.Entities;

namespace GA360.DAL.Infrastructure.Interfaces;

public interface IDocumentRepository : IRepository<Document>
{
    Task UpsertDocuments(int userId, List<Document> documents);
    Task<bool> CleanOrphans(int userId, List<Document> documents);
}
