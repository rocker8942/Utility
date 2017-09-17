using System.Collections.Generic;

namespace Utility.FTP
{
    public interface ISFtpClient
    {
        /// <summary>
        /// Upload file through FTP 
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="remotePath">Remote path needs to be separate with '/'</param>
        /// <param name="preserveTimestamp"></param>
        /// <returns></returns>
        List<string> FtpUpload(string localPath, string remotePath, bool preserveTimestamp = true);
    }
}