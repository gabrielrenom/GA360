using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Entities.Entities;

public class Document: Audit, IModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public int Status { get; set; }
    public int Tags { get; set; }
    public string Description { get; set; }
    public string FileType { get; set; }
    public string FileSize { get; set; }
    public string Path { get; set; }
}
