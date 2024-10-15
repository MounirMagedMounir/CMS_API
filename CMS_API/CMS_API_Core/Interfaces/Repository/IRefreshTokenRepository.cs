using CMS_API_Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.Interfaces.Repository
{
    public interface IRefreshTokenRepository
    {
        void AddRefreshToken(RefreshToken refreshToken);
        void DeleteRefreshToken(RefreshToken refreshTokenId);
        Task SaveChangesAsync();
    }
}
