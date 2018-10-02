using System;
using System.Collections.Generic;
using System.Text;

namespace SecureChat.Core.FileRetrieval
{
    interface IRetrievalBase
    {
        /// <summary>
        /// Gets the data from a file.
        /// </summary>
        /// <returns>String of Data from a file.</returns>
        string GetData();
    }
}
