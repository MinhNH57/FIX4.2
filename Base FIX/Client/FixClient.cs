
using QuickFix;
using QuickFix.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary2.FIXF;
using ClassLibrary2;

namespace Client
{
    public class FixClient : MessageCracker, IApplication
    {

        #region Base App
        public void OnCreate(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) => Console.WriteLine("Đăng nhập thành công" + sessionID + "\n");
        public void OnLogout(SessionID sessionID) => Console.WriteLine("Đăng xuất " + sessionID);
        public void ToAdmin(Message message, SessionID sessionID) { Console.WriteLine("To Admin:" +Class1.BuildStringFIX1(message.ToString() , typeof(f)) + "\n"); }
        public void ToApp(Message message, SessionID sessionID) { Console.WriteLine("To App : " + Class1.BuildStringFIX1(message.ToString(), typeof(f)) + "\n"); }
        public void FromAdmin(Message message, SessionID sessionID) { Console.WriteLine("From Admin :" + Class1.BuildStringFIX1(message.ToString(), typeof(f)) + "\n"); }
        public void FromApp(Message message, SessionID sessionID) => Console.WriteLine("From App :" + Class1.BuildStringFIX1(message.ToString(), typeof(f)) + "\n");
        #endregion

        public void SendNewOrderSingle(SessionID sessionID)
        {
            var clOrdID = Guid.NewGuid();

            Console.Write("Nhập tên cổ phiếu: ");
            var symbol = Console.ReadLine();

            Console.Write("Mua hay bán (1:mua - 2:bán): ");
            var sideInput = Console.ReadLine();
            var side = sideInput == "1" ? Side.BUY : Side.SELL;

            Console.WriteLine("Nhập số tiền :");
            var tien = Console.ReadLine();

            Console.Write("Nhập số luwngh ");
            var orderQtyInput = Console.ReadLine();
            int orderQty;
            while (!int.TryParse(orderQtyInput, out orderQty) || orderQty <= 0)
            {
                Console.Write("Nhap so nguyen duong dei");
                orderQtyInput = Console.ReadLine();
            }

            var message = new QuickFix.FIX42.NewOrderSingle(
                new ClOrdID(clOrdID.ToString()),
                new HandlInst('1'), 
                new Symbol(symbol),
                new Side(side),
                new TransactTime(DateTime.Now),
                new OrdType(OrdType.MARKET_ON_CLOSE)
            );
            message.Set(new Price(decimal.Parse(tien)));
            message.Set(new OrderQty(orderQty));

            try
            {
                Session.SendToTarget(message, sessionID);
                Console.WriteLine("Gửi lệnh đặt mới thành công.");
            }
            catch (SessionNotFound ex)
            {
                Console.WriteLine("Lỗi oyyyy: " + ex.Message);
            }
        }

        public void SenNewCancelOrder(SessionID sessionID)
        {
            Console.WriteLine("Nhap ID cua lenh muon xoa :");
            var origClOrdID = Console.ReadLine();
            var clOrdID = Guid.NewGuid().ToString();
            //Console.WriteLine("Ten Co phieu");
            //var symbol = Console.ReadLine();
            var cancelRequest = new QuickFix.FIX42.OrderCancelRequest(
                new OrigClOrdID(origClOrdID), 
                new ClOrdID(clOrdID),  
                new Symbol("FPT"),  
                new Side(Side.BUY),                  
                new TransactTime(DateTime.Now)     
            );

            try
            {
                Session.SendToTarget(cancelRequest, sessionID);
                Console.WriteLine("Gửi yêu cầu hủy lệnh thành công.");
            }
            catch (SessionNotFound ex)
            {
                Console.WriteLine("Lỗi: Không tìm thấy session. " + ex.Message);
            }
        }

    }
}
