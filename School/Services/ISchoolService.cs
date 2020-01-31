using School.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services
{
    public interface ISchoolService
    {
        Response<SchoolModel> GetSchool(long currentUserId);
    }
}
