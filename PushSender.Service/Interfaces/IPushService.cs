using PushSender.Service.Enums;
using PushSender.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Service.Interfaces
{
    public interface IPushService
    {
        void Send(IEnumerable<Device> devicesList, string message);
    }
}
