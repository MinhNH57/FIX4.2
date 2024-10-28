using QuickFix.Transport;
using QuickFix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFix.Fields;
namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = Encoding.UTF8;
            int sess;
            var settings = new SessionSettings(@"C:\Users\Dell\Desktop\FIX4.2\Base FIX\Client\Fix.cfg");
            var application = new FixClient();
            var storeFactory = new FileStoreFactory(settings);
            var logFactory = new FileLogFactory(settings);
            var initiator = new SocketInitiator(application, storeFactory, settings, logFactory);

            if(DateTime.Now.TimeOfDay > TimeSpan.Parse("12:59:59"))
            {
                sess = 0;
            }
            else
            {
                sess = 1;
            }

            initiator.Start();
            while (true)
            {
                var currentTime = DateTime.Now.TimeOfDay;
                var endTimeMorning = TimeSpan.Parse("11:29:59");
                var endTimeAfternoon = TimeSpan.Parse("14:59:59");

                if (currentTime > endTimeMorning && sess == 1)
                {
                    sess = 0;
                    initiator.Stop();
                    Console.WriteLine("Session is closed");
                    Console.ReadLine();
                }

                if(currentTime > endTimeAfternoon && sess == 0)
                {
                    sess = 1;
                    initiator.Stop();
                    Console.WriteLine("Session is closed");
                    Console.ReadLine();
                }
                Console.WriteLine("Client đã khởi động. Chọn một hành động:");
                Console.WriteLine("1 - Gửi lệnh mới");
                Console.WriteLine("2 - Hủy lệnh");
                Console.WriteLine("3 - Thoát");
                Console.Write("Chọn hành động (1, 2, hoặc 3): \n ");

                string choice = Console.ReadLine();
                var sessionID = new SessionID("FIX.4.2", "CLIENT1", "SERVER");

                switch (choice)
                {
                    case "1":
                        application.SendNewOrderSingle(sessionID);
                        Console.WriteLine("Đã gửi lệnh mới.");
                        break;

                    case "2":
                        application.SenNewCancelOrder(sessionID);
                        Console.WriteLine("Đã gửi yêu cầu hủy lệnh.");
                        break;

                    case "3":
                        initiator.Stop();
                        Console.WriteLine("Dừng client...");
                        return;

                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }

                Console.WriteLine("Nhấn <Enter> để tiếp tục...");
                Console.ReadLine();
            }
        }
    }
}
