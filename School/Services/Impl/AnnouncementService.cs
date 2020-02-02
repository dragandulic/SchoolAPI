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

        public Response<long> AddAnnouncement(AnnouncementModel model, long currentPersonId)
        {
            var response = new Response<long>();
            try
            {
                var classOfPerson = _schoolContext.ClassPerson.Where(c => c.PersonId == currentPersonId && c.Class.Active == true).FirstOrDefault().ClassId;
                var announcemnt = new Announcement()
                {
                    Title = model.Title,
                    Description = model.Description,
                    ClassId = classOfPerson,
                    PersonId = currentPersonId,
                    Time = DateTime.Now
                };
                _schoolContext.Announcement.Add(announcemnt);
                _schoolContext.SaveChanges();
                response.Value = announcemnt.Id;
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        Response<List<AnnouncementModel>> IAnnouncementService.GetAll(long personId)
        {
            var response = new Response<List<AnnouncementModel>>()
            {
                Value = new List<AnnouncementModel>()
            };
            var classOfPerson = _schoolContext.ClassPerson.Where(c => c.PersonId == personId && c.Class.Active == true).FirstOrDefault().ClassId;
            response.Value.AddRange(_schoolContext.Announcement.Where(a => a.ClassId == classOfPerson).Select(a => new AnnouncementModel
            {
                Title = a.Title,
                Description = a.Description,
                Time = a.Time.Value.ToString("dd.MM.yyyy. HH:mm"),
                CreatedBy = a.Person.FirstName + " " + a.Person.LastName
            }).ToList());
            return response;
        }
    }
}
