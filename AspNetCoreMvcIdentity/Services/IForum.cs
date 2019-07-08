using AspNetCoreMvcIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Services
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();

        Task Create(Forum forum);
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
        bool HasRecentPost(int id);
        IEnumerable<ApplicationUser> GetAllActiveUsers(int id);
    }
}
