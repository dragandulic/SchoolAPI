using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Contracts.Responses
{
    public class DeviceTokenModel
    {
        public string DeviceToken { get; set; }
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
