using CMS_API_Core.DomainModels.Authentication;
using CMS_API_Core.Interfaces.Repository.Authentication;
using CMS_API_Infrastructure.DBcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository.Authentication
{

    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext _context;

        public RefreshTokenRepository(DataContext context)
        {
            _context = context;
        }
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshToken.Add(refreshToken);
        }

        public void DeleteRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshToken.Remove(refreshToken);
        }

        public RefreshToken GetRefreshToken(string refreshToken)
        {
            var token = _context.RefreshToken.FirstOrDefault(x => x.Token == refreshToken);
            return token;
        }

        public void SaveChanges()
        {

            _context.SaveChanges();
        }

        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
           _context.RefreshToken.Update(refreshToken);
        }
    }
}
