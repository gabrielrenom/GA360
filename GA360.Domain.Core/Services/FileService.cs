using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GA360.Commons.Settings;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;

namespace GA360.Domain.Core.Services;

public class FileService:IFileService
{
    private BlobStorageSettings _blobSettings;

    public FileService(IOptions<BlobStorageSettings> blobSettings)
    {
        _blobSettings = blobSettings.Value;
    }

    public async Task<IList<FileModel>> UploadDocumentsAsync(IList<FileModel> files, string folderName)
    {
        var blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.DocumentsContainerName);

        // Create the container if it doesn't exist
        await blobContainerClient.CreateIfNotExistsAsync();

        foreach (var file in files)
        {
            if (file.ByteArrayContent != null)
            {
                var blobClient = blobContainerClient.GetBlobClient($"{folderName}/{file.Name}");

                using (var memoryStream = new MemoryStream(file.ByteArrayContent))
                {
                    await blobClient.UploadAsync(memoryStream, true);
                }

                file.Url = blobClient.Uri.ToString();
                file.BlobId = blobClient.Name;
                file.Name = blobClient.Name;
            }
        }

        return files;
    }

    public async Task DeleteDocumentBlobAsync(string blobId)
    {
        var blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.DocumentsContainerName);
        var blobClient = blobContainerClient.GetBlobClient(blobId);
        await blobClient.DeleteAsync();
    }

    public static async Task<IList<FileModel>> ExtractFiles(List<IFormFile> formFiles)
    {
        var fileModels = new List<FileModel>();

        try
        {
            // Process the uploaded files
            foreach (var file in formFiles)
            {
                if (file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        // Copy the content to a byte array
                        var byteArrayContent = memoryStream.ToArray();

                        // Create a FileModel instance and add it to the list
                        var fileModel = new FileModel
                        {
                            Name = file.FileName,
                            ByteArrayContent = byteArrayContent
                        };
                        fileModels.Add(fileModel);
                    }
                }
                else
                {
                    fileModels.Add(new FileModel { Name = file.FileName, BlobId = file.FileName });
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception as needed
            throw;
        }

        return fileModels;
    }


    public async Task<IList<FileModel>> CleanOrphans(IList<FileModel> files, string folderName)
    {
        var blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.DocumentsContainerName);

        var blobsToDelete = new List<FileModel>();

        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync(prefix: folderName + "/"))
        {
            if (!files.Any(f => f.BlobId == blobItem.Name))
            {
                blobsToDelete.Add(new FileModel { BlobId = blobItem.Name });
                await blobContainerClient.DeleteBlobIfExistsAsync(blobItem.Name);
            }
        }

        return blobsToDelete;
    }
}
