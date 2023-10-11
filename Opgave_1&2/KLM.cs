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
    internal class KLM
    {

        public static void Req_ETA (ETA_request ETA_request)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ETA_request));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, ETA_request);
            string xmlMessage = stringWriter.ToString();

            MessageQueue messageQueue = new MessageQueue(@".\Private$\KLM");

            messageQueue.Send(xmlMessage);
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
