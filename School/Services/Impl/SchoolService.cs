using School.Contracts.Responses;
using School.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services.Impl
{
    public class SchoolService : ISchoolService
    {
        private readonly SchoolContext _schoolContext;

        public SchoolService(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }
        public Response<SchoolModel> GetSchool(long currentUserId)
        {
            var response = new Response<SchoolModel>();
            var personClass = _schoolContext.ClassPerson.Where(c => c.PersonId == currentUserId && c.Class.Active == true).Select(c=>c.Class).FirstOrDefault();
            var personAcademy = _schoolContext.Class.Where(c => c.Id == personClass.Id).Select(c=>c.Academy).FirstOrDefault();
            response.Value = new SchoolModel()
            {
                Name = personAcademy.Name,
                Description = personAcademy.Description,
                Location = personAcademy.Location
            };

            return response;
        }
    }
}
