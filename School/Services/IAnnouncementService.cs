using School.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IAnnouncementService
    {
        Response<List<AnnouncementModel>> GetAll(long personId);
        Response<AnnouncementModel> AddAnnouncement(AnnouncementModel model, long currentPersonId);
    }
}
