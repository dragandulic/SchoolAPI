using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using School.Contracts.Responses;
using School.Data;

namespace School.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubjectsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SchoolContext _context;


        public SubjectsController(UserManager<ApplicationUser> userManager, SchoolContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        [HttpGet("api/GetSubjectsForUser")]
        public async Task<IActionResult> GetSubjectsForUser()
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var class_person = _context.ClassPerson.Where(c => c.PersonId == user.PersonId).Select(s => new { s.Mark, s.ClassId, s.PersonId }).Single();
            string grades = class_person.Mark;
            Console.WriteLine(grades);
            var response = new ClassPersonModel();
            response.ClassId = class_person.ClassId;
            response.PersonId = class_person.PersonId;
            response.Grades = grades;
            return Json(response);

        }

        [HttpGet("api/GetSubjectsForAllUsers")]
        public async Task<IActionResult> GetSubjectsForAllUsers()
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var classId = _context.ClassPerson.Where(cp => cp.PersonId == user.PersonId && cp.Class.Active == true).FirstOrDefault().ClassId;
            var students = _context.ClassPerson.Where(cp => cp.ClassId == classId).Select(cp => new ClassPersonModel()
            {
                ClassId = cp.ClassId,
                Grades = cp.Mark,
                PersonId = cp.PersonId
            }).ToList();

            return Json(students);
        }

        [HttpPost("api/ChangeGrades")]
        public async Task<IActionResult> ChangeGradesOfUser([FromBody] ClassPersonModel model)
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var classId = _context.ClassPerson.Where(cp => cp.PersonId == user.PersonId && cp.Class.Active == true).FirstOrDefault().ClassId;
            var classPerson = _context.ClassPerson.Where(cp => cp.ClassId == classId && cp.PersonId == Convert.ToInt64(model.Personidstring)).FirstOrDefault();
            classPerson.Mark = model.Grades;
            sendNotification(classPerson.PersonId);
            _context.SaveChanges();

            return Json("");
        }

        public void sendNotification(long personId)
        {
            var person = _context.Person.Where(c => c.Id == personId).FirstOrDefault();


            try
            {

                string applicationID = "AAAA1EfZ36A:APA91bErsR3LiWc-vLLigrFG0aMovC9jN3nChyAauxgqcgg_NEEqvkXEMoUJs7d5uHhlDQw5DdXxfjPCXHOmbWJw81yGCfSJcpbcXnxM0Iqm-qM1LEOQErneqEISsyDNoCF7VsW2IAQc";

                string senderId = "911738527648";

                string deviceId = person.DeviceId;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "You have new grades!",
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