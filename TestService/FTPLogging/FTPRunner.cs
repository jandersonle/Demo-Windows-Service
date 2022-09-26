using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.FTPLoggingService
{
    public class FTPRunner
    {
        public FTPRunner()
        {
            Main();
        }

        public static void Main()
        {
            // ftp helper controls the logging protocols uusing the FTPConnectoin class
            _ = new FTPHelper();
        }
    }
}
