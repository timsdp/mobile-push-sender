using PushSender.Service.Enums;
using PushSender.Service.Interfaces;
using PushSender.Service.Models;
using PushSender.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Console.App
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "Hello world!";

            //Init context
            ServiceContext context = new ServiceContext()
            {
                AppleCertFileName = "",
                AppleCertPassword="123",
                FcmUrl= "https://fcm.googleapis.com/fcm/send",
                FcmApiKey="123"
            };

            //Get devices
            List<Device> devices = new List<Device>
            {
                new Device(DeviceType.Android,"asdfsd765645df4sd56fas67d5fa75s4f65asdf"),
                new Device(DeviceType.Android,"reg5kj345kjhg24j5k234"),
                new Device(DeviceType.IOS,"234jhn5k2hg45h2345j43"),
                new Device(DeviceType.IOS,"j24h35kjh2345jkn2345"),
                new Device(DeviceType.Android,"j43h5k23gh452hj345h23k45hjg2k3j45"),
            };

            //Init service
            IPushService service = new SenderService(context);

            //Send
            service.Send(devices,message);
        }
    }
}
