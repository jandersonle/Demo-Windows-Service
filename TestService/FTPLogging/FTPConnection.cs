using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestService.FTPLoggingService
{
    internal class FTPConnection
    {
        private String server;
        private String userName;
        private String pwd;

        private FtpWebRequest _request;


        public FTPConnection(String iserver, String iusername = "", String ipwd = "")
        {
            this.server = iserver;
            this.userName = iusername;
            this.pwd = ipwd;
            InitConnection();
        }

        private void InitConnection()
        {
            try
            {
                // This assumes the FTP site uses anonymous logon.
                this._request = (FtpWebRequest)WebRequest.Create(this.server);
                this._request.Credentials = new NetworkCredential(this.userName, this.pwd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public String GetDirectoryContents(String directoryPath = "")
        {
            _request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            FtpWebResponse response = (FtpWebResponse)_request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            //Console.WriteLine(reader.ReadToEnd());

            String directoryResults = reader.ReadToEnd();



            reader.Close();
            response.Close();

            return directoryResults;
        }


        public String ReadFile(String fileName)
        {
            String content = "";
            try
            {
                this._request = (FtpWebRequest)WebRequest.Create(this.server + "\\" + fileName);
                this._request.Method = WebRequestMethods.Ftp.DownloadFile;

                // Read the file from the server & write to destination                
                using (FtpWebResponse response = (FtpWebResponse)_request.GetResponse()) // Error here
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {

                    content += (reader.ReadToEnd());
                }
            }
            catch (WebException e)
            {

                content = e.Message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                content = e.Message;
                return content;
            }

            return content;
        }


        public void AppendToFile(String textToAppend, String filename)
        {
            byte[] data = Encoding.ASCII.GetBytes(textToAppend);

            try
            {
                this._request = (FtpWebRequest)WebRequest.Create(this.server + "\\" + filename);
                _request.Method = WebRequestMethods.Ftp.AppendFile;
                _request.ContentLength = data.Length;
                Stream requestStream = _request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)_request.GetResponse();
                response.Close();
            }
            catch (Exception e)
            {
                return;
            }
        }

        public async void AddFile(String localPath)
        {
            try
            {
                InitConnection();
                _request.Method = WebRequestMethods.Ftp.UploadFile;

                using (FileStream fileStream = File.Open(localPath, FileMode.Open, FileAccess.Read))
                {
                    using (Stream requestStream = _request.GetRequestStream())
                    {
                        await fileStream.CopyToAsync(requestStream);
                        using (FtpWebResponse response = (FtpWebResponse)_request.GetResponse())
                        {
                            //Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

        }

        public bool CheckIfFileExists(String fileName)
        {

            this._request = (FtpWebRequest)WebRequest.Create(this.server + "\\" + fileName);
            _request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)_request.GetResponse();
                return true;
            }
            catch (WebException e)
            {
                FtpWebResponse response = (FtpWebResponse)e.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;

        }


        public String DeleteFile(String fileName)
        {
            this._request = (FtpWebRequest)WebRequest.Create(this.server + "\\" + fileName);
            _request.Method = WebRequestMethods.Ftp.DeleteFile;

            using (FtpWebResponse response = (FtpWebResponse)_request.GetResponse())
            {
                return response.StatusDescription;
            }
        }
    }
}

