using PushSender.Service.Enums;
using PushSender.Service.Interfaces;
using PushSender.Service.Models;
using PushSender.Service.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Service.Service
{
    public class SenderService : IPushService
    {
        ServiceContext context;
        public SenderService(ServiceContext context)
        {
            this.context = context;
        }
        public void Send(IEnumerable<Device> devicesList, string message)
        {
            IEnumerable<DeviceType> types = devicesList.Select(d => d.Type).Distinct();
            foreach (DeviceType type in types)
            {
                ISender sender = getSender(type);
                if (sender!=null)
                {
                    IEnumerable<string> idsList = devicesList.Where(d => d.Type == type).Select(d => d.Id);
                    sender.Send(idsList, message);
                }
            }
        }

        private ISender getSender(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.IOS:
                    return new IOSSender(context);
                case DeviceType.Android:
                    return new AndroidSender(context);
                case DeviceType.Unknown:
                default:
                    return null;
            }
        }
    }
}
