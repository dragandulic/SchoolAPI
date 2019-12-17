using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class Subject
    {
        public Subject()
        {
            ClassSubject = new HashSet<ClassSubject>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long ProfessorId { get; set; }
        public long AcademyId { get; set; }

        public virtual Academy Academy { get; set; }
        public virtual Person Professor { get; set; }
        public virtual ICollection<ClassSubject> ClassSubject { get; set; }
    }
}
