using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Opgave_1_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ETA_request msgETA = new ETA_request("KLM112");
            KLM.Req_ETA(msgETA);
            Console.WriteLine(msgETA.ToString());

            MessageQueue messageQueue = new MessageQueue(@".\Private$\KLM");

            Message receivedMessage = messageQueue.Receive();
            string receivedXmlMessage = receivedMessage.Body.ToString();
            ETA_request receivedETAMessage;
            XmlSerializer serializer = new XmlSerializer(typeof(ETA_request));


            using (StringReader stringReader = new StringReader(receivedXmlMessage))
            {
                receivedETAMessage = (ETA_request)serializer.Deserialize(stringReader);
            }

            List<Fly> flyList = new List<Fly>();
            flyList.Add(new Fly("KLM122", "15:11"));
            flyList.Add(new Fly("UNI122", "15:30"));
            flyList.Add(new Fly("KLM912", "15:45"));

            foreach (Fly fly in flyList)
            {
                if (fly.Id == receivedETAMessage.Flight_No)
                {
                    ETA_reply message = new ETA_reply(fly.ETA);
                    message
                }
            }

        }
    }

    public class Fly
    {
        public string Id;
        
        public string ETA;

        public Fly(string id, string eta) 
        { 
            this.Id = id;
            this.ETA = eta;
        }
    }

    [Serializable]
    public class ETA_reply
    {
        public string ETA;

        public ETA_reply(string ETA)
        {
            this.ETA = ETA;
        }
    }
}
