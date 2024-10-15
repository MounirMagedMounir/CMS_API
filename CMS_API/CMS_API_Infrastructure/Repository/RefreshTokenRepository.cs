using CMS_API_Core.DomainModels;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Infrastructure.DBcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Infrastructure.Repository
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

        public async Task SaveChangesAsync()
        {

            _context.SaveChanges();
        }


    }
}
