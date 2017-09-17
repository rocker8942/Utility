using Utility.FTP;
using WinSCP;

namespace Example
{
    /// <summary>
    /// FTP client use example
    /// </summary>
    class FTP
    {
        /// <summary>
        /// Send file over non secure FTP 
        /// </summary>
        public void SendSftp()
        {
            var sftpClient = new SFtpClient(new SessionOptions(), "hostname", "username", "password", Protocol.Ftp);

            sftpClient.FtpUpload("localPath", "remotePath");

        }
    }
}
