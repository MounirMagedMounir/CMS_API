using CMS_API_Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.Interfaces.Repository
{
    public interface ISessionRepository
    {
        Session GetSessionByUserId(string userId);
        Session GetSessionByToken(string Token);
        void AddSession(Session session);
        void DeleteSession(Session session);
        Task SaveChangesAsync();
    }
}
