using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Xml.Serialization;
using System.IO;

namespace KLM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ETA_request message = new ETA_request("KLM112");

            XmlSerializer serializer = new XmlSerializer(typeof(ETA_request));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, message);
            string xmlMessage = stringWriter.ToString();

            MessageQueue messageQueue = new MessageQueue(@".\Private$\KLM");

            messageQueue.Send(xmlMessage);

            Message receivedMessage = messageQueue.Receive();
            string receivedXmlMessage = receivedMessage.Body.ToString();
            ETA_request receivedETAMessage;

            using (StringReader stringReader = new StringReader(receivedXmlMessage))
            {
                receivedETAMessage = (ETA_request)serializer.Deserialize(stringReader);
            }




        }
    }

    [Serializable]
    public class ETA_request
    {
        public string Flight_No;
        public ETA_request() { }

        public ETA_request(string flight_No)
        {
            Flight_No = flight_No;
        }
    }
}
