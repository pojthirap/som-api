using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.sendGrid
{
    public class SendMailModel 
    {
        public string SenderName { get; set; }
        public List<string> MailTo { get; set; }
        public bool showAllRecipients { get; set; }
        public List<string> MailCC { get; set; }
        public List<string> MailBCC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MeetingModel Meeting { get; set; }


    }

   public class MeetingModel
    {
        public string Organizer { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
    }
}
