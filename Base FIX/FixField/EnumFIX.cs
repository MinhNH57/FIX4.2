using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixField
{
    public enum EnumFIX
    {
        BeginString = 8,
        MsgType = 35,
        MsgSeqNum = 34,
        SenderCompID = 49,
        TargetCompID = 56,
        SendingTime = 52,
        Side = 54,
        Symbol = 55,
        OrderQty = 38,
        OrdType = 40,
        Price = 44,
        ExecType = 150,
        LeavesQty = 151,
        CumQty = 14,
        LastShares = 32,
        LastPx = 31,
        LastMkt = 155,
        TransactTime = 60,
        AvgPx = 100,
        OrigClOrdID = 119,
        ClearingFirm = 220,
        CheckSum = 10,
        OrderID = 23,
        TimeInForce = 59,
        SettlType = 218
    }
}
