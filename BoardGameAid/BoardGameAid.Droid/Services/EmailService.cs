using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using BoardGameAid.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace BoardGameAid.Droid.Services
{
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Send an email with a given address and subject.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        public void SendEmail(string emailAddress, string subject)
        {
            // let's include the device info in the body
            string body = $"\n\n_____________\nType your email above the line\n" +
                          $"Phone model: {Build.Manufacturer} {Build.Model} | {Build.CpuAbi}\n" +
                          $"Android version info: {Build.VERSION.Release} | {Build.VERSION.Incremental}";
                          
            SendEmail(emailAddress, subject, body);
        }

        /// <summary>
        /// Send an email with a given address, subject and initial body.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void SendEmail(string emailAddress, string subject, string body)
        {
            var email = new Intent(Intent.ActionSend);
            email.SetType("message/rfc822");
            email.PutExtra(Intent.ExtraEmail, new[] { emailAddress });
            email.PutExtra(Intent.ExtraSubject, subject);
            email.PutExtra(Intent.ExtraText, body);
            try
            {
                var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                top.Activity.StartActivity(email);

            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
