using CMS_API_Core.DomainModels;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context)
        {
            _context = context;
        }
        public void AddSession(Session session)
        {
            _context.Session.Add(session);
        }

        public void DeleteSession(Session session)
        {
            _context.Session.Remove(session);
        }

        public Session GetSessionByToken(string Token)
        {
            return _context.Session.Include(u => u.User).Include(u => u.RefreshToken).FirstOrDefault(u => u.Token == Token);
        }

        public Session GetSessionByUserId(string userId)
        {
            return _context.Session.Include(r => r.RefreshToken).FirstOrDefault(u => u.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {

            _context.SaveChanges();
        }


    }
}
