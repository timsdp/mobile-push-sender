using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Service.Interfaces
{
    public interface ISender
    {
        bool Send(IEnumerable<string> deviceIdList, string message);
    }
}
