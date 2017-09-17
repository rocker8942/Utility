using System;
using System.Collections.Generic;
using WinSCP;

namespace Utility.FTP
{
    public class SFtpClient : Ftp, ISFtpClient
    {
        private readonly SessionOptions _sessionOptions;

        /// <summary>
        /// FTP connection 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="protocol"></param>
        public SFtpClient(string hostname, string username, string password, Protocol protocol )
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = protocol,
                HostName = hostname,
                UserName = username,
                Password = password,
            };
        }

        /// <summary>
        /// FTP connection 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="protocol"></param>
        /// <param name="timeout"></param>
        /// <param name="ftpMode"></param>
        public SFtpClient(string hostname, string username, string password, Protocol protocol, TimeSpan timeout, FtpMode ftpMode)
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = protocol,
                HostName = hostname,
                UserName = username,
                Password = password,
                Timeout = timeout,
                FtpMode = ftpMode
            };
        }

        /// <summary>
        /// SFTP connection with password 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="hostKey"></param>
        public SFtpClient(string hostname, string username, string password, string hostKey = "")
        {
            if (string.IsNullOrEmpty(hostKey))
            {
                _sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = hostname,
                    UserName = username,
                    Password = password,
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };
            }
            else
            {
                _sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = hostname,
                    UserName = username,
                    Password = password,
                    SshHostKeyFingerprint = hostKey
                };
            }
        }

        /// <summary>
        /// SFTP connection with SSH Key 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="hostKey"></param>
        /// <param name="privateKeyPath"></param>
        public SFtpClient(string hostname, string username, string password, string hostKey, string privateKeyPath)
        {
            if (string.IsNullOrEmpty(hostKey))
            {
                _sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = hostname,
                    UserName = username,
                    Password = password,
                    GiveUpSecurityAndAcceptAnySshHostKey = true,
                    SshPrivateKeyPath = privateKeyPath,
                };
            }
            else
            {
                _sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = hostname,
                    UserName = username,
                    Password = password,
                    SshHostKeyFingerprint = hostKey,
                    SshPrivateKeyPath = privateKeyPath,
                };
            }
        }

        /// <summary>
        /// FTPs connection 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ftpSecure"></param>
        /// <param name="portNumber"></param>
        public SFtpClient(string hostname, string username, string password, FtpSecure ftpSecure, int portNumber)
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = hostname,
                UserName = username,
                Password = password,
                FtpSecure = ftpSecure,
                PortNumber = portNumber,
                GiveUpSecurityAndAcceptAnyTlsHostCertificate = true
            };
        }

        /// <summary>
        /// Upload file through FTP 
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="remotePath">Remote path needs to be separate with '/'</param>
        /// <param name="preserveTimestamp"></param>
        /// <returns></returns>
        public List<string> FtpUpload(string localPath, string remotePath, bool preserveTimestamp = true)
        {
            var uploadedFiles = new List<string>();

            using (var session = new Session())
            {
                // Connect
                session.Open(_sessionOptions);

                // Upload files
                var transferOptions = new TransferOptions
                {
                    TransferMode = TransferMode.Binary,
                    PreserveTimestamp = preserveTimestamp
                };

                TransferOperationResult transferResult = session.PutFiles(localPath, remotePath, false, transferOptions);

                // Throw on any error
                transferResult.Check();

                // Print results
                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    uploadedFiles.Add(transfer.FileName);
                }
            }

            return uploadedFiles;
        }
    }
}