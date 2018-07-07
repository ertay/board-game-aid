using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAid.Core.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Send an email with a given address and subject.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        void SendEmail(string emailAddress, string subject);

        /// <summary>
        /// Send an email with a given address, subject and initial body.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        void SendEmail(string emailAddress, string subject, string body);
    }
}
