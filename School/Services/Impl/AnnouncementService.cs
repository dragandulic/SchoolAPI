using Microsoft.AspNetCore.Mvc;
using School.Contracts.Responses;
using School.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace School.Services.Impl
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly SchoolContext _schoolContext;

        public AnnouncementService(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        public Response<AnnouncementModel> AddAnnouncement(AnnouncementModel model, long currentPersonId)
        {
            var response = new Response<AnnouncementModel>();
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
                sendNotification(currentPersonId);

                var person = _schoolContext.Person.Find(currentPersonId);

                var announcementModel = new AnnouncementModel()
                {
                    Title = announcemnt.Title,
                    Description = announcemnt.Description,
                    Time = announcemnt.Time.Value.ToString("dd.MM.yyyy. HH:mm"),
                    CreatedBy = person.FirstName + " " + person.LastName
                };

                response.Value = announcementModel;
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

        public void sendNotification(long currentId)
        {
            var current_person = _schoolContext.Person.Where(c => c.Id == currentId).FirstOrDefault();
            var classId = _schoolContext.ClassPerson.Where(cp => cp.PersonId == currentId && cp.Class.Active == true).FirstOrDefault().ClassId;
            var students = _schoolContext.ClassPerson.Where(p => p.ClassId == classId).Select(s => new DeviceTokenModel
            {
                Id = s.Person.Id,
                DeviceToken = s.Person.DeviceId,
                FirstName = s.Person.FirstName,
                LastName = s.Person.LastName
            }).ToList();



            for (var i = 0; i < students.Count; i++)
            {
                
                if(students[i].Id != currentId)
                {

                    try
                    {

                        string applicationID = "AAAA1EfZ36A:APA91bErsR3LiWc-vLLigrFG0aMovC9jN3nChyAauxgqcgg_NEEqvkXEMoUJs7d5uHhlDQw5DdXxfjPCXHOmbWJw81yGCfSJcpbcXnxM0Iqm-qM1LEOQErneqEISsyDNoCF7VsW2IAQc";

                        string senderId = "911738527648";

                        string deviceId = students[i].DeviceToken;

                        WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                        tRequest.Method = "post";
                        tRequest.ContentType = "application/json";
                        var data = new
                        {
                            to = deviceId,
                            notification = new
                            {
                                body = current_person.FirstName +" "+current_person.LastName+" made announcement!",
                                title = "School app",
                                sound = "Enabled"

                            }
                        };
                        var serializer = new JavaScriptSerializer();
                        var json = serializer.Serialize(data);
                        Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                        tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                        tRequest.ContentLength = byteArray.Length;
                        using (Stream dataStream = tRequest.GetRequestStream())
                        {
                            dataStream.Write(byteArray, 0, byteArray.Length);
                            using (WebResponse tResponse = tRequest.GetResponse())
                            {
                                using (Stream dataStreamResponse = tResponse.GetResponseStream())
                                {
                                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                    {
                                        String sResponseFromServer = tReader.ReadToEnd();
                                        string str = sResponseFromServer;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }








                }
            }
        }
    }
}
