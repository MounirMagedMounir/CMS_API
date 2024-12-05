using CMS_API_Core.DomainModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.Interfaces.Repository.Authentication
{
    public interface IRefreshTokenRepository
    {
        void AddRefreshToken(RefreshToken refreshToken);
        void DeleteRefreshToken(RefreshToken refreshTokenId);
        RefreshToken GetRefreshToken(string refreshToken);
        void UpdateRefreshToken(RefreshToken refreshToken);
        void SaveChanges();
    }
}
