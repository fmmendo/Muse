using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace MuseBackgroundTask
{
    public sealed class NewsNotifierBackgroundTask : IBackgroundTask
    {
        private const string NEWSRSS = "http://muse.mu/rss/news/";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            if (CanSendToasts())
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("newsCache");
                var text = await FileIO.ReadTextAsync(file);

                var items = new List<string>();

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(NEWSRSS))
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();

                    if (result == null) return;

                    var xmlItems = System.Xml.Linq.XElement.Parse(result);
                    foreach (var item in xmlItems.Descendants("item"))
                    {
                        var title = System.Net.WebUtility.HtmlDecode(item.Element("title").Value);
                        if (!text.Contains(title))
                        {
                            items.Add(title);
                        }
                    }
                }

                if (items.Count == 0)
                    return;

                ToastNotification toast = null;
                if (items.Count == 1)
                    toast = CreateTextOnlyToast("News", String.Format(items[0]));
                if (items.Count > 1)
                    toast = CreateTextOnlyToast("News", String.Format("You have {0} unread news items", items.Count));

                toast.ExpirationTime = DateTimeOffset.UtcNow.AddDays(1);

                // Display the toast.
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }

            _deferral.Complete();
        }

        public static bool CanSendToasts()
        {
            bool canSend = true;
            var notifier = ToastNotificationManager.CreateToastNotifier();

            if (notifier.Setting != NotificationSetting.Enabled)
            {
                string reason = "unknown";
                switch (notifier.Setting)
                {
                    case NotificationSetting.DisabledByGroupPolicy:
                        reason = "An administrator has disabled all notifications on this computer through group policy. The group policy setting overrides the user's setting.";
                        break;
                    case NotificationSetting.DisabledByManifest:
                        reason = "To be able to send a toast, set the Toast Capable option to \"Yes\" in this app's Package.appxmanifest file.";
                        break;
                    case NotificationSetting.DisabledForApplication:
                        reason = "The user has disabled notifications for this app.";
                        break;
                    case NotificationSetting.DisabledForUser:
                        reason = "The user or administrator has disabled all notifications for this user on this computer.";
                        break;
                }

                string errroMessage = String.Format("Can't send a toast.\n{0}", reason);
                //MainPage.Current.NotifyUser(errroMessage, NotifyType.ErrorMessage);
                canSend = false;
            }

            return canSend;
        }

        public static ToastNotification CreateTextOnlyToast(string toastHeading, string toastBody)
        {
            // Using the ToastText02 toast template.
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;

            // Retrieve the content part of the toast so we can change the text.
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            //Find the text component of the content
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");

            // Set the text on the toast. 
            // The first line of text in the ToastText02 template is treated as header text, and will be bold.
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastHeading));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastBody));

            // Set the duration on the toast
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            // Create the actual toast object using this toast specification.
            ToastNotification toast = new ToastNotification(toastXml);

            return toast;
        }
    }
}
