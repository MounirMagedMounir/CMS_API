using CMS_API_Core.DomainModels.Authentication;

namespace CMS_API_Core.Interfaces.Repository.Authentication
{
    public interface ISessionRepository
    {
        Session GetSessionByUserId(string userId);
        Session GetSessionByToken(string Token);
        void AddSession(Session session);
        void UpdateSession(Session session);
        void DeleteSession(Session session);
        void SaveChanges();
    }
}
