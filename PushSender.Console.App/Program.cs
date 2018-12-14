using Microsoft.Extensions.Configuration;
using PushSender.Service.Enums;
using PushSender.Service.Interfaces;
using PushSender.Service.Models;
using PushSender.Service.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.ConsoleApp
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        static void Main(string[] args)
        {
            string defaultConfigFileName = "config-default.json";
            string realConfigFileName = "config.json";

            string configFileName = File.Exists(realConfigFileName) ? realConfigFileName : defaultConfigFileName;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(configFileName);
            Configuration = builder.Build();

            string message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            //Init context
            ServiceContext context = new ServiceContext()
            {
                AppleCertFileName = Configuration["ios:certfilename"],
                AppleCertPassword = Configuration["ios:certpass"],
                FcmUrl= Configuration["android:fcmurl"],
                FcmApiKey= Configuration["android:apikey"]
            };

            //Get devices
            List<Device> devices = new List<Device>
            {
                new Device(DeviceType.Android,"fJ8hGwWzwM4:APA91bFoIb2Znpbu4RG5_znXCu576mTPbf6LhO4QYBYQONujyYDW6opVOyBLzN_Esj763nbIoPUBY2FpGnGsK4VVxXm2ALeykYxJ5HoJnPAulIt1fOElrTIIbXdBoe4SmVW0adej0SWl"),
                //new Device(DeviceType.Android,"reg5kj345kjhg24j5k234"),
                //new Device(DeviceType.IOS,"234jhn5k2hg45h2345j43"),
                //new Device(DeviceType.IOS,"j24h35kjh2345jkn2345"),
                //new Device(DeviceType.Android,"j43h5k23gh452hj345h23k45hjg2k3j45"),
            };

            //Init service
            IPushService service = new SenderService(context);

            //Send
            service.Send(devices,message);

            Console.ReadLine();
        }
    }
}
