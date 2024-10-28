using QuickFix;
using QuickFix.Fields;
using System;
using System.Security.Policy;
using ClassLibrary2;
using ClassLibrary2.FIXF;

public class FixServer : MessageCracker, IApplication
{
    public void OnCreate(SessionID sessionID) { }

    public void OnLogon(SessionID sessionID) => Console.WriteLine("Người dùng đã đăng nhập \n" + sessionID);

    public void OnLogout(SessionID sessionID) => Console.WriteLine("Người dùng đã cút \n" + sessionID);

    public void ToAdmin(Message message, SessionID sessionID) { Console.WriteLine("ToAdmin - Sending administrative message:" + Class1.BuildStringFIX1(message.ToString() , typeof(f))); }

    public void ToApp(Message message, SessionID sessionID) {
        Console.WriteLine($"ToApp - Sending application-level message: {message.ToString()}\n");
    }

    public void FromAdmin(Message message, SessionID sessionID) { Console.WriteLine("FromAdmin - Received administrative message:" + Class1.BuildStringFIX1(message.ToString(), typeof(f))); }

    public void FromApp(Message message, SessionID sessionID)
    {
        try
        {
            Console.WriteLine("Received message from client: \n" + message);
            Crack(message, sessionID);
        }
        catch(QuickFix.UnsupportedMessageType ex)
        {
            Console.WriteLine("Unsupported message type: \n" + ex.Message);
        }
    }

    public void OnMessage(QuickFix.FIX42.NewOrderSingle order, SessionID sessionID)
    {
        Console.WriteLine("Received NewOrderSingle:");
        Console.WriteLine("OderID :" +order.ClOrdID.getValue());
        Console.WriteLine("Symbol: " + order.Symbol.getValue());
        Console.WriteLine("OrderQty: " + order.OrderQty.getValue());
        Console.WriteLine("Side: " + order.Side.getValue());
        Console.WriteLine("CreatAtTime : " +order.TransactTime);

        // Chỗ này để làm việc với cơ sở dữ liệu

        var orderID = new OrderID(order.ClOrdID.getValue());
        var execID = new ExecID("54321");
        var execTransType = new ExecTransType(ExecTransType.NEW);
        var execType = new ExecType(ExecType.FILL);
        var ordStatus = new OrdStatus(OrdStatus.FILLED);
        var symbol = order.Symbol;
        var side = order.Side;
        var leavesQty = new LeavesQty(0);
        var cumQty = new CumQty(order.OrderQty.getValue());
        var avgPx = new AvgPx((decimal)100.0);

        var executionReport = new QuickFix.FIX42.ExecutionReport(
            orderID,
            execID,
            execTransType,
            execType,
            ordStatus,
            symbol,
            side,
            leavesQty,
            cumQty,
            avgPx
        );

        Session.SendToTarget(executionReport, sessionID);
    }

    public void OnMessage(QuickFix.FIX42.OrderCancelRequest message, SessionID sessionID)
    {
        Console.WriteLine("Received OrderCancelRequest:");
        Console.WriteLine("OrigClOrdID: " + message.OrigClOrdID.getValue());
        Console.WriteLine("ClOrdID: " + message.ClOrdID.getValue());
        Console.WriteLine("Symbol: " + message.Symbol.getValue());
        Console.WriteLine("Side: " + message.Side.getValue());

        var executionReport = new QuickFix.FIX42.ExecutionReport(
            new OrderID(message.OrigClOrdID.getValue()),  
            new ExecID("54321"),   
            new ExecTransType(ExecTransType.NEW), 
            new ExecType(ExecType.CANCELED),  
            new OrdStatus(OrdStatus.CANCELED), 
            message.Symbol,    
            message.Side,                     
            new LeavesQty(0),   
            new CumQty(0),                   
            new AvgPx(0)          
        );

        Session.SendToTarget (executionReport, sessionID);
    }
}
