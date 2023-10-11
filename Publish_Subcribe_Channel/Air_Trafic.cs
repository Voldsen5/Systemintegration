using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Publish_Subcribe_Channel
{
    internal class Air_Trafic
    {
        public void Send(string destination, Message message)
        {
            string queuePath = @".\Private$\" + destination;

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
