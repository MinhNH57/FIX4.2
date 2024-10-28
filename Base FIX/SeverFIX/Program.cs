using QuickFix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverFIX
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                var settings = new SessionSettings(@"C:\Users\Dell\Desktop\FIX4.2\Base FIX\SeverFIX\Fix.cfg");
                var application = new FixServer();
                var storeFactory = new FileStoreFactory(settings);
                var logFactory = new FileLogFactory(settings);
                var acceptor = new ThreadedSocketAcceptor(application, storeFactory, settings, logFactory);

                acceptor.Start();
                Console.WriteLine("Server started. Press <Enter> to stop...");
                Console.ReadLine();  

                acceptor.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Press <Enter> to exit...");
                Console.ReadLine();  
            }

        }
    }
}
