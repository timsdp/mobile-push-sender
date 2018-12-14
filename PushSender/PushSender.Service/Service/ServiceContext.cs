using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Service.Service
{
    public class ServiceContext
    {
        public string GcmSenderId { get;  set; }
        public string GcmAuthToken { get;  set; }
        public string FcmApiKey { get;  set; }
        public string FcmUrl { get;  set; }
        public string AppleCertFileName { get;  set; }
        public string AppleCertPassword { get;  set; }
    }
}
