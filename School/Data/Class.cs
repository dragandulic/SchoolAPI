using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class Class
    {
        public Class()
        {
            Announcement = new HashSet<Announcement>();
            ClassPerson = new HashSet<ClassPerson>();
            ClassSubject = new HashSet<ClassSubject>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public long AcademyId { get; set; }
        public string Generation { get; set; }

        public virtual Academy Academy { get; set; }
        public virtual ICollection<Announcement> Announcement { get; set; }
        public virtual ICollection<ClassPerson> ClassPerson { get; set; }
        public virtual ICollection<ClassSubject> ClassSubject { get; set; }
    }
}
