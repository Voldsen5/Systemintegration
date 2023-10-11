using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Content_Filter
{
    internal class Content_Filter
    {
        protected MessageQueue MessageQueueIn;
        protected MessageQueue MessageQueueOut;

        public Content_Filter(MessageQueue _messageQueueIn, MessageQueue _messageQueueOut) 
        {
            this.MessageQueueIn = _messageQueueIn;
            this.MessageQueueOut = _messageQueueOut;

            _messageQueueIn.ReceiveCompleted += new ReceiveCompletedEventHandler(OnMessage);
            _messageQueueIn.BeginReceive();
        }

        private void OnMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)source;
            Message message = mq.EndReceive(asyncResult.AsyncResult);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(message.BodyStream);

            double FullWeight = 0.0;

            foreach (XmlNode luggageNode in xmlDocument.SelectNodes("//FlightDetailsInfoResponse/Flight/Passenger/Luggage"))
            {
                XmlNode weightNode = luggageNode.SelectSingleNode("Weight");

                if (weightNode != null)
                {
                    string weightValue = weightNode.InnerText;
                    if (Double.TryParse(weightValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double luggageWeight))
                    {
                        FullWeight += luggageWeight; 
                    }
                    else
                    {
                        throw new Exception("Vægt kunne parses til en double.");
                    }
                }
                else
                {
                    throw new Exception("Vægt node ikke fundet i Baggage noden ;).");
                }
            }
            Message messageOut = new Message();
            string mess = "" + FullWeight;
            messageOut.Body = mess;
            MessageQueueOut.Send(messageOut);

            mq.BeginReceive();
            //Console.WriteLine("Den fulde vægt af baggage: " + FullWeight + " Wup Wup");
        }
    }
}
