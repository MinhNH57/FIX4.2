using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Example.CreateFIXMessage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateMess fixMessage = new CreateMess();

            Console.WriteLine("Nhap begin :");
            string beginString = Console.ReadLine();
            Console.WriteLine("Nhap sender :");
            string senderComId = Console.ReadLine();
            Console.WriteLine("Nhap message type :");
            string ms = Console.ReadLine();
            Console.WriteLine("Nhap sequenceNumber : ");
            int sequenceNumber = int.Parse(Console.ReadLine());

            string message = fixMessage.CreateMessage(beginString, senderComId, "EXECUTOR", ms, sequenceNumber);
            Console.WriteLine("Thong diep");
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
