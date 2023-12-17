using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Entities;

namespace Inventory.APIAuthorization.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}