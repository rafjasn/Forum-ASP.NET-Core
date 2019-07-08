using AspNetCoreMvcIdentity.Data;
using AspNetCoreMvcIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Services
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public ApplicationUser GetById(long id)
        {
            return GetAll().FirstOrDefault(u => u.Id == id);
        }

        public async Task UpdateUserRating(string id, Type type)
        {
            var longid = Convert.ToInt64(id);
            var user = GetById(longid);
            user.Rating = CalculateUserRating(type, user.Rating);
          
            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;
            if (type == typeof(Post))
                inc = 1;
            if (type == typeof(PostReply))
                inc = 3;

            return userRating + inc;
        }

        public async Task SetProfileImage(long id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
