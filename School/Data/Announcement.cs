using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class Announcement
    {
        public Announcement()
        {
            Comment = new HashSet<Comment>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Time { get; set; }
        public long PersonId { get; set; }
        public long? ClassId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
    }
}
