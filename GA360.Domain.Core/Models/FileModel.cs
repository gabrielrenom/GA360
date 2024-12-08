namespace GA360.Domain.Core.Models;

public class FileModel
{
    public string Name { get; set; }
    public MemoryStream Content { get; set; }
    public string Url { get; set; }
    public string BlobId { get; set; }
    public byte[] ByteArrayContent { get; set; }
}
