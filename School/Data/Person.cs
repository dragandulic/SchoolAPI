using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class Person
    {
        public Person()
        {
            Announcement = new HashSet<Announcement>();
            AspNetUsers = new HashSet<AspNetUsers>();
            ClassPerson = new HashSet<ClassPerson>();
            Comment = new HashSet<Comment>();
            InverseMyParent = new HashSet<Person>();
            Subject = new HashSet<Subject>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? MyParentId { get; set; }
        public string ImageUrl { get; set; }

        public string DeviceId { get; set; }

        public virtual Person MyParent { get; set; }
        public virtual ICollection<Announcement> Announcement { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<ClassPerson> ClassPerson { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Person> InverseMyParent { get; set; }
        public virtual ICollection<Subject> Subject { get; set; }
    }
}
