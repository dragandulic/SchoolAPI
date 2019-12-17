using System;
using System.Collections.Generic;

namespace School.Data
{
    public partial class ClassPerson
    {
        public long ClassId { get; set; }
        public long PersonId { get; set; }
        public string Mark { get; set; }

        public virtual Class Class { get; set; }
        public virtual Person Person { get; set; }
    }
}
