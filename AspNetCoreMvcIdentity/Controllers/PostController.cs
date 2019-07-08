using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreMvcIdentity.Models;
using AspNetCoreMvcIdentity.Models.PostViewModels;
using AspNetCoreMvcIdentity.Models.ReplyViewModels;
using AspNetCoreMvcIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcIdentity.Controllers
{
    
    public class PostController : Controller
    {
        private readonly IForum _forum;
        private readonly IPost _post;
        private readonly IApplicationUser _user;

        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost post, IForum forum, UserManager<ApplicationUser> userManager, IApplicationUser user)
        {
            _post = post;
            _forum = forum;
            _userManager = userManager;
            _user = user;
        }

   
        
        public IActionResult Index(int id)
        {
            var post = _post.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id.ToString(),
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                AuthorImageUrl = post.User.ProfileImageUrl,
                IsAuthorAdmin = IsAuthorAdmin(post.User)

            };
            return View(model);
        }

        private bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(r => new PostReplyModel
            {
                Id = r.Id,
                AuthorName = r.User.UserName,
                AuthorId = r.User.Id.ToString(),
                AuthorImageUrl = r.User.ProfileImageUrl,
                AuthorRating = r.User.Rating,
                Created = r.Created,
                ReplyContent = r.Content,
                IsAuthorAdmin = IsAuthorAdmin(r.User)
            });
        }
        ///////////////////////////////////////////////////////////////////////////
        [Authorize]
        public IActionResult Create(int id)
        {
            var forum = _forum.GetById(id);

            var model = new NewPostModel
                {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name
                

                };

            return View(model);
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;

            var post = _post.GetById(id);
            var model = new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                DatePosted = post.Created.ToString()
                
            };

            return View(model);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirmed(int id)
        {

            var redirect = _post.GetById(id).Forum.Id;

            _post.Delete(id);
            return RedirectToAction("Details", "Forum", new { id = redirect   });


        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user =  _userManager.GetUserAsync(User).Result;
            var post = BuildPost(model, user);
             _post.Add(post).Wait();
            await _user.UpdateUserRating(userId, typeof(Post));
            //TODO Implement user rating
            return RedirectToAction("Index", "Post", new { id = post.Id });
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forum.GetById(model.ForumId);


            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum

            };
        }
    }
}