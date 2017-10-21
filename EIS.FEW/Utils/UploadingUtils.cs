using System;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace DevExpress.Web.Demos {
    public static class UploadingUtils {
        const string RemoveTaskKeyPrefix = "DXRemoveTask_";

        public static void RemoveFileWithDelay(string key, string fullPath, int delay) {
            RemoveFileWithDelayInternal(key, fullPath, delay, FileSystemRemoveAction);
        }
        public static void RemoveFileFromAzureWithDelay(string fileKeyName, string accountName, string accessKey, string containerName, int delay) {
            AzureFileInfo azureFile = new AzureFileInfo(fileKeyName, accountName, accessKey, containerName);
            RemoveFileWithDelayInternal(fileKeyName, azureFile, delay, AzureStorageRemoveAction);
        }
        public static void RemoveFileFromAmazonWithDelay(string fileKeyName, string accessKeyID, string secretAccessKey, string bucketName, string region, int delay) {
            AmazonFileInfo amazonFile = new AmazonFileInfo(fileKeyName, accessKeyID, secretAccessKey, bucketName, region);
            RemoveFileWithDelayInternal(fileKeyName, amazonFile, delay, AmazonStorageRemoveAction);
        }
        public static void RemoveFileFromDropboxWithDelay(string fileKeyName, string AccessTokenValue, int delay)
        {
            DropboxFileInfo dropboxFile = new DropboxFileInfo(fileKeyName, AccessTokenValue);
            RemoveFileWithDelayInternal(fileKeyName, dropboxFile, delay, DropboxStorageRemoveAction);
        }
        static void FileSystemRemoveAction(string key, object value, CacheItemRemovedReason reason) {
            string fileFullPath = value.ToString();
            if(File.Exists(fileFullPath))
                File.Delete(fileFullPath);
        }
        static void AzureStorageRemoveAction(string key, object value, CacheItemRemovedReason reason) {
            AzureFileInfo azureFile = value as AzureFileInfo;
            if(azureFile != null) {
                AzureFileSystemProvider provider = new AzureFileSystemProvider("");
                provider.StorageAccountName = azureFile.StorageAccountName;
                provider.AccessKey = azureFile.AccessKey;
                provider.ContainerName = azureFile.ContainerName;
                FileManagerFile file = new FileManagerFile(provider, azureFile.FileKeyName);
                provider.DeleteFile(file);
            }
        }
        static void AmazonStorageRemoveAction(string key, object value, CacheItemRemovedReason reason) {
            AmazonFileInfo amazonFile = value as AmazonFileInfo;
            if(amazonFile != null) {
                AmazonFileSystemProvider provider = new AmazonFileSystemProvider("");
                provider.AccessKeyID = amazonFile.AccessKeyID;
                provider.SecretAccessKey = amazonFile.SecretAccessKey;
                provider.BucketName = amazonFile.BucketName;
                provider.Region = amazonFile.Region;
                FileManagerFile file = new FileManagerFile(provider, amazonFile.FileKeyName);
                provider.DeleteFile(file);
            }
        }
        static void DropboxStorageRemoveAction(string key, object value, CacheItemRemovedReason reason)
        {
            DropboxFileInfo dropboxFile = value as DropboxFileInfo;
            if (dropboxFile != null)
            {
                DropboxFileSystemProvider provider = new DropboxFileSystemProvider("");
                provider.AccessTokenValue = dropboxFile.AccessTokenValue;
                FileManagerFile file = new FileManagerFile(provider, dropboxFile.FileKeyName);
                provider.DeleteFile(file);
            }
        }
        static void RemoveFileWithDelayInternal(string fileKey, object fileData, int delay, CacheItemRemovedCallback removeAction) {
            string key = RemoveTaskKeyPrefix + fileKey;
            if(HttpRuntime.Cache[key] == null) {
                DateTime absoluteExpiration = DateTime.UtcNow.Add(new TimeSpan(0, delay, 0));
                HttpRuntime.Cache.Insert(key, fileData, null, absoluteExpiration,
                    Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, removeAction);
            }
        }

        class AzureFileInfo {
            public string FileKeyName { get; set; }
            public string StorageAccountName { get; set; }
            public string AccessKey { get; set; }
            public string ContainerName { get; set; }

            public AzureFileInfo(string fileKeyName, string storageAccountName, string accessKey, string containerName) {
                FileKeyName = fileKeyName;
                StorageAccountName = storageAccountName;
                AccessKey = accessKey;
                ContainerName = containerName;
            }
        }

        class AmazonFileInfo {
            public string FileKeyName { get; set; }
            public string AccessKeyID { get; set; }
            public string SecretAccessKey { get; set; }
            public string BucketName { get; set; }
            public string Region { get; set; }

            public AmazonFileInfo(string fileKeyName, string accessKeyID, string secretAccessKey, string bucketName, string region) {
                FileKeyName = fileKeyName;
                AccessKeyID = accessKeyID;
                SecretAccessKey = secretAccessKey;
                BucketName = bucketName;
                Region = region;
            }
        }

        class DropboxFileInfo
        {
            public string FileKeyName { get; set; }
            public string AccessTokenValue { get; set; }

            public DropboxFileInfo(string fileKeyName, string accessTokenValue)
            {
                FileKeyName = fileKeyName;
                AccessTokenValue = accessTokenValue;
            }            
        }
    }
}
