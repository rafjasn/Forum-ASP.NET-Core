using AspNetCoreMvcIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Services
{
   public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);


        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int id, string newContent);
        Task AddReply(PostReply reply);
        IEnumerable<Post> GetLatestPosts(int n);
        IEnumerable<Post> GetMostPopularPosts(int n);
    }
}
