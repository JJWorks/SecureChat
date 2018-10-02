using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace SecureChat.Core.FileRetrieval
{
    public class AzureCloudFileRetrieval : IRetrievalBase
    {
        /// <summary>
        /// Connection String to Azure Files.
        /// </summary>
        private string _ConnectionString;

        /// <summary>
        /// Directory Location to Azure.
        /// </summary>
        private string _DirectoryLocation;

        /// <summary>
        /// File Name to Read.
        /// </summary>
        private string _FileName;

        /// <summary>
        /// Retrieves File Content from an Azure File System.
        /// </summary>
        /// <param name="ConnectionString">Connection String to the Azure File System.</param>
        /// <param name="ShareLocation">Location of the Share (folder).</param>
        /// <param name="FileName">File Name to Read.</param>
        public AzureCloudFileRetrieval(string ConnectionString, string ShareLocation, string FileName)
        {
            _ConnectionString = ConnectionString;
            _DirectoryLocation = ShareLocation;
            _FileName = FileName;
        }

        /// <summary>
        /// Retrieves Files from Azure File System.
        /// </summary>
        /// <param name="ConnectionInformation">Dictionary&lt;string,string&gt; with connectionstring, sharelocation and filetouse</param>
        /// <returns>File in a String.</returns>
        public string GetData()
        {
            var temp = GetData_Async();
            return temp.Result;
        }


        /// <summary>
        /// Async Method to get Data from Azure Cloud.
        /// </summary>
        /// <returns>String of the file.</returns>
        private async Task<string> GetData_Async()
        {
            string DataToShare = string.Empty;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_ConnectionString);
            // Create a new file share, if it does not already exist.
            // Create a CloudFileClient object for credentialed access to File storage.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference(_DirectoryLocation);
            if (await share.ExistsAsync())
            {
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                if (await rootDir.ExistsAsync())
                {
                    CloudFile FiletoUse = rootDir.GetFileReference(_FileName);
                    if (await FiletoUse.ExistsAsync())
                    {
                        DataToShare = await FiletoUse.DownloadTextAsync();
                    }
                }
            }
            return DataToShare;
        }


    }
}
