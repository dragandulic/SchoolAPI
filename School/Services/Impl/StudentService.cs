using School.Contracts.Responses;
using School.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services.Impl
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _entity;

        public StudentService(SchoolContext schoolContext)
        {
            _entity = schoolContext;
        }

        public Response<List<UserModel>> GetStudentsFromMyClass(long? personId, List<long> teachers)
        {
            var response = new Response<List<UserModel>>()
            {
                Value = new List<UserModel>()
            };


            try
            {

                var classId = _entity.ClassPerson.Where(cp => cp.PersonId == personId && cp.Class.Active == true).FirstOrDefault().ClassId;
                var students = _entity.ClassPerson.Where(p => p.ClassId == classId && !teachers.Contains(p.PersonId)).Select(s => new UserModel
                {
                    Id = s.Person.Id,
                    FirstName = s.Person.FirstName,
                    LastName = s.Person.LastName,
                    Email = s.Person.AspNetUsers.FirstOrDefault().Email,
                    PhoneNumber = s.Person.AspNetUsers.FirstOrDefault().PhoneNumber,
                    ImageUrl = s.Person.ImageUrl
                }).ToList();

                response.Value.AddRange(students);
                response.Status = "Success";
            }
            catch (Exception ex)
            {
                response.Status = "Error";
            }
            return response;
        }

        
    }
}
