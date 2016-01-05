using NiceLife.Schedule.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace NiceLife.Schedule
{
    public sealed class PlanTask : IBackgroundTask
    {
        List<Call> listc;
        List<Plan> listp;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
           
               await GetPlan();
          
              deferral.Complete();
        }
        private async Task<string> Remind()
        {
            CallHelper cp = CallHelper.GetHelper();
             listc = cp.SelectlistItems(DateTime.Today);
            for(int i = 0; i < listc.Count(); i++)
            {
                if (listc.ElementAt(i).Date == DateTime.Now)
                {
                    PlanHelper ph = PlanHelper.GetHelper();
                    listp = ph.SelectGroupItems(listc.ElementAt(i).Id);
                    for (int j = 0; j < listp.Count(); j++) {
                        ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                        XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                        XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                        toastTextElements[0].AppendChild(toastXml.CreateTextNode(listp.ElementAt(j).Title+":"+ listp.ElementAt(j).Description));
                        ToastNotification toast = new ToastNotification(toastXml);
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
            }
                return null;
        }
        private IAsyncOperation<string> GetPlan()
        {
            try
            {
                return AsyncInfo.Run(token => Remind());
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }
   
}
