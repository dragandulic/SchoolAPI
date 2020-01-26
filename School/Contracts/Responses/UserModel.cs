﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Contracts.Responses
{
    public class UserModel
    {

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}
