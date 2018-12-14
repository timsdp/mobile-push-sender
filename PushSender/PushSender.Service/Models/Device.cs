using PushSender.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Service.Models
{
    public class Device
    {
        public DeviceType Type { get; set; }
        public string Id { get; set; }

        public Device(DeviceType type, string id)
        {
            this.Type = type;
            this.Id = id;
        }
    }
}
