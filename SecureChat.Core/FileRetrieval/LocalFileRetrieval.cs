using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SecureChat.Core.FileRetrieval
{
    public class LocalFileRetrieval : IRetrievalBase
    {
        /// <summary>
        /// Gets Content of a File in the File System.
        /// </summary>
        /// <param name="FullFileName">Full File Name of the file to read.</param>
        public LocalFileRetrieval(string FullFileName)
        {
            FileLocation = FullFileName;
        }

        /// <summary>
        /// Location of the File.
        /// </summary>
        private string FileLocation;

        /// <summary>
        /// Gets Full Data from Flat File
        /// </summary>
        /// <returns>Content of The Data</returns>
        public string GetData()
        {
            string contentsoffile = string.Empty;
            using (var r = new StreamReader(FileLocation))
            {
                contentsoffile = r.ReadToEnd();
            }
            return contentsoffile;
        }
    }
}
