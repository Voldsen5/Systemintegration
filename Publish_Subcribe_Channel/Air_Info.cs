using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Publish_Subcribe_Channel
{
    internal class Air_Info
    {
        public Message Receive(string receiveFrom)
        {
            string queuePath = @".\Private$\" + receiveFrom;

            if (!MessageQueue.Exists(queuePath))
            {
                throw new InvalidOperationException("Queue does not exist.");
            }

            using (MessageQueue mQ = new MessageQueue(queuePath))
            {
                mQ.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                Message message = mQ.Receive();
                return message;
            }
        }

        public void Redirect(Message message)
        {
            string Airline = message.Body.ToString();

           if  (Airline.ToUpper().Trim() == "KLM")
            {
                string queuePath = @".\Private$\" + Airline;

                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(queuePath);
                }

                using (MessageQueue mQ = new MessageQueue(queuePath))
                {
                    mQ.Send(message);
                }
            }
        }
    }
}
