using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Contracts.Responses
{
    public class AnnouncementModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string CreatedBy { get; set; }
    }
}
