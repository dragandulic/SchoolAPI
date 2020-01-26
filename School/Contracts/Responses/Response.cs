using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Contracts.Responses
{
    public class Response<T>
    {
        public T Value { get; set; }
        public string Status { get; set; }
    }
}
