using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSender.Service.Interfaces;
using PushSender.Service.Service;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSender.Service.Sender
{
    public class AndroidSender : ISender
    {
        ServiceContext context;
        public AndroidSender(ServiceContext context)
        {
            this.context = context;
        }
        public bool Send(IEnumerable<string> deviceIdList, string message)
        {
            // Configuration GCM (use this section for GCM)
            //var config = new GcmConfiguration(context.GcmSenderId, context.GcmAuthToken, null);
            //var provider = "GCM";

            // Configuration FCM (use this section for FCM)
            var config = new GcmConfiguration(context.FcmApiKey);
            config.GcmUrl = context.FcmUrl;
            var provider = "FCM";

            // Create a new broker
            var gcmBroker = new GcmServiceBroker(config);

            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) => {

                aggregateEx.Handle(ex => {

                    // See what kind of exception it was to further diagnose
                    if (ex is GcmNotificationException notificationException)
                    {

                        // Deal with the failed notification
                        var gcmNotification = notificationException.Notification;
                        var description = notificationException.Description;

                        Console.WriteLine($"{provider} Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                    }
                    else if (ex is GcmMulticastResultException multicastException)
                    {

                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            Console.WriteLine($"{provider} Notification Succeeded: ID={succeededNotification.MessageId}");
                        }

                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var n = failedKvp.Key;
                            var e = failedKvp.Value;

                            Console.WriteLine($"{provider} Notification Failed: ID={n.MessageId}, Desc={e.Message}");
                        }

                    }
                    else if (ex is DeviceSubscriptionExpiredException expiredException)
                    {

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;

                        Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                            // If this value isn't null, our subscription changed and we should update our database
                            Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                        }
                    }
                    else if (ex is RetryAfterException retryException)
                    {

                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        Console.WriteLine($"{provider} Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                    }
                    else
                    {
                        Console.WriteLine("{provider} Notification Failed for some unknown reason");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            gcmBroker.OnNotificationSucceeded += (notification) => {
                Console.WriteLine("{provider} Notification Sent!");
            };

            // Start the broker
            gcmBroker.Start();

            foreach (var regId in deviceIdList)
            {
                // Queue a notification to send
                gcmBroker.QueueNotification(new GcmNotification
                {
                    RegistrationIds = new List<string> {
            regId
        },
                    //Data = JObject.Parse("{ \"somekey\" : \"somevalue\" }"),
                    Notification = JObject.Parse($"{{ \"title\" : \"title\", \"body\" : \"{message}\"}}"),
                });
            }

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            gcmBroker.Stop();
            return true;
        }
    }
}
