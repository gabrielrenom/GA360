using GA360.Domain.Core.Models;

namespace GA360.Domain.Core.Interfaces;

public interface IFileService
{
    Task DeleteDocumentBlobAsync(string blobId);
    Task<IList<FileModel>> UploadDocumentsAsync(IList<FileModel> files, string folderName);
    Task<IList<FileModel>> CleanOrphans(IList<FileModel> files, string folderName);
}
