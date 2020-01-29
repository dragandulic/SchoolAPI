using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Contracts.Responses
{
    public class ClassPersonModel
    {
        public long ClassId { get; set; }
        public long PersonId { get; set; }
        public string Grades { get; set; }
    }
}
