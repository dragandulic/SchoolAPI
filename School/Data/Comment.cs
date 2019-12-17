using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class Comment
    {
        public long Id { get; set; }
        public DateTime? Time { get; set; }
        public long AnnouncementId { get; set; }
        public long PersonId { get; set; }
        public string Text { get; set; }

        public virtual Announcement Announcement { get; set; }
        public virtual Person Person { get; set; }
    }
}
