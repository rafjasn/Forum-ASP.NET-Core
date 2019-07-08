using AspNetCoreMvcIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Services
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(long id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(long id, Uri uri);
        Task UpdateUserRating(string id, Type type);

    }
}
