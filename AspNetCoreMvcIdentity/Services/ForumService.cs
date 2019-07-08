using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcIdentity.Data;
using AspNetCoreMvcIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMvcIdentity.Services
{
    public class ForumService : IForum
    {

        private readonly ApplicationDbContext _context;

        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums
                .Include(f => f.Posts);
        }

   

        public IEnumerable<ApplicationUser> GetAllActiveUsers(int id)
        {
            var posts = GetById(id).Posts;
            if (posts != null || !posts.Any())
            {
                var postUsers = posts.Select(p => p.User);
                var replyUsers = posts.SelectMany(p => p.Replies).Select(r => r.User);

                return postUsers.Union(replyUsers).Distinct();
            }
            return new List<ApplicationUser>();
        }

        public Forum GetById(int id)
        {
            return _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                    .ThenInclude(r => r.User)
                .FirstOrDefault();

        }

        public bool HasRecentPost(int id)
        {
            const int hoursAgo = 24;
            var window = DateTime.Now.AddHours(-hoursAgo);
            return GetById(id).Posts.Any(p => p.Created > window);
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }
    }
}
