using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RecipientList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\Volds\\source\\repos\\Systemintegration\\RecipientList\\pas1.xml");

            MessageQueue mq = new MessageQueue(@".\Private$\RecipientList");
            
            mq.Send(doc);
            Console.ReadLine();
            RecipientList recipientList = new RecipientList(mq);


        }
    }
}
