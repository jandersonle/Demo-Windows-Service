using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.FTPLoggingService
{
    internal class FTPHelper
    {

        private FTPConnection _fTPConnection;

        private string server;

        private string password;

        private string username;


        public FTPHelper()
        {
            this.server = "ftp://10.1.0.6:21/";
            this.username = "";
            this.password = "";
            run();
        }


        public FTPHelper(string server, string password, string username)
        {
            
            this.server = server;
            this.password = password;
            this.username = username;
            run();
        }

        private void run()
        {
            
            EstablishConnection();

            //ClearLoggingFile();

            //determine if the log exists, if it does not exist, create it. Then Append an entry to it
            bool logExists = CheckLogExists();

            if (!logExists)
            {
                CreateLoggingFile();
            }

            GenericTimer<Object> timer = new GenericTimer<Object>(this.AppendToLoggingFile, 2000, 3);
            timer.Start();

            // Console.WriteLine("Logging complete...");
            //DisplayLogContents();
        }

        private void EstablishConnection()
        {
            // assumes an anonymous connection protocol  
            this._fTPConnection = new FTPConnection(this.server, this.username, this.password);
        }

        private String GetDirectoryContents()
        {
            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            String directoryResults = this._fTPConnection.GetDirectoryContents("");
            return directoryResults;
        }

        private bool CheckLogExists()
        {
            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            bool logExists = this._fTPConnection.CheckIfFileExists("log.txt");
            return logExists;
        }

        private void CreateLoggingFile()
        {
            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            //uploads a log template file to the server
            this._fTPConnection.AddFile("C:\\Users\\Justin.Anderson\\source\\repos\\FTPConnectionService\\FTPConnectionService\\log.txt");
        }

        public Object AppendToLoggingFile()
        {
            String entry = CreateEntry();

            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            this._fTPConnection.AppendToFile(entry, "log.txt");
            return null;
        }


        private void DisplayLogContents()
        {
            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            String logContent = this._fTPConnection.ReadFile("log.txt");
            //Console.WriteLine(logContent);
        }

        private void DeleteLoggingFile()
        {
            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            String result = this._fTPConnection.DeleteFile("log.txt");
            //Console.WriteLine(result);
        }

        private void ClearLoggingFile()
        {
            if (this._fTPConnection == null)
            {
                throw new NullReferenceException();
            }

            DeleteLoggingFile();

            String header = "Logging information for demo service " + "\n";
            header += (
                "==========================================================\n"
                );

            this._fTPConnection.AppendToFile(header, "log.txt");

        }



        private static String CreateEntry()
        {
            DateTime now = DateTime.Now;
            String entry = "Log entry for " + now.ToString() + "\n";
            entry += (
                "==========================================================\n"
                );

            return entry;
        }
    }
}
