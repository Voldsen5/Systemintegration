using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Publish_Subcribe_Channel
{
    internal class Airline
    {
        public string Name { get; set; }

        public Airline(string _name) 
        {
        this.Name = _name;
        }

        public Message Receive()
        {
            string queuePath = @".\Private$\" + this.Name;

            if (!MessageQueue.Exists(queuePath))
            {
                throw new InvalidOperationException("Queue does not exist.");
            }

            using (MessageQueue mQ = new MessageQueue(queuePath))
            {
                mQ.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                Message message = mQ.Receive();
                if (message.Body.ToString() == this.Name) 
                { 
                    return message;
                }
            }

            return null;
        }

    }
}
