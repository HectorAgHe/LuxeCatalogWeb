using LuxeCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
