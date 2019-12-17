using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class Academy
    {
        public Academy()
        {
            Class = new HashSet<Class>();
            Subject = new HashSet<Subject>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Class> Class { get; set; }
        public virtual ICollection<Subject> Subject { get; set; }
    }
}
