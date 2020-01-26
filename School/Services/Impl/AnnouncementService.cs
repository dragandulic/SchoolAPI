using Microsoft.AspNetCore.Mvc;
using School.Contracts.Responses;
using School.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services.Impl
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly SchoolContext _schoolContext;

        public AnnouncementService(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        Response<List<AnnouncementModel>> IAnnouncementService.GetAll(long personId)
        {
            var response = new Response<List<AnnouncementModel>>() 
            { 
                Value = new List<AnnouncementModel>()
            };
            var classOfPerson = _schoolContext.ClassPerson.Where(c => c.PersonId == personId && c.Class.Active == true).FirstOrDefault().ClassId;
            response.Value.AddRange(_schoolContext.Announcement.Where(a=>a.ClassId==classOfPerson).Select(a => new AnnouncementModel
            {
                Title = a.Title,
                Description = a.Description
            }).ToList());
            return response;
        }
    }
}
