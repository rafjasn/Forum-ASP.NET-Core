using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcIdentity.Models;
using AspNetCoreMvcIdentity.Models.ForumViewModels;
using AspNetCoreMvcIdentity.Models.PostViewModels;
using AspNetCoreMvcIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcIdentity.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forum;
        private readonly IPost _post;

        public ForumController(IForum forum, IPost post)
        {
            _forum = forum;
            _post = post;
        }

        public IActionResult Index()
        {
            var forums = _forum.GetAll().Select(f => new ForumListingModel
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                NumberOfPosts = f.Posts?.Count() ?? 0,
                NumberOfUsers = _forum.GetAllActiveUsers(f.Id).Count(),
                ForumImageUrl = f.ImageUrl,
                HasRecentPost = _forum.HasRecentPost(f.Id)
            });

            var model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(f => f.Title)
            };


            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var forum = _forum.GetById(id);
            var model = BuildForumListing(forum);

            return View(model);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public  IActionResult DeleteConfirmed(int id)
        {
            

            _forum.Delete(id);
            return RedirectToAction("Index");


        }




        public IActionResult Details(int id, string searchQuery)
        {
            var posts = new List<Post>();
            var forum = _forum.GetById(id);
        
                 posts = _post.GetFilteredPosts(forum, searchQuery).ToList();
       
            var postListings = posts.Select(p => new PostListingModel
            {
                Id = p.Id,
                AuthorId = p.User.Id.ToString(),
                AuthorRating = p.User.Rating,
                Title = p.Title,
                DatePosted = p.Created.ToString(),
                RepliesCount = p.Replies.Count(),
                AuthorName = p.User.UserName,
                Forum = BuildForumListing(p)


            });

            var model = new ForumDetailsModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum),
                SearchQuery = searchQuery
            };



            return View(model);
        }

    
        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Details", new { id, searchQuery });

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new AddForumModel();

            return View(model);

        }
        [HttpPost]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/forum/forum.png";
            //if (model.ImageUpload != null)
            //{
            //    var blocBlob = UploadForumImage(model.ImageUpload);
            //    imageUri = blocBlob.Uri.AbsoluteUri;

            //}

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };
            await _forum.Create(forum);
            return RedirectToAction("Index", "Forum");
      
        }

        private object UploadForumImage(IFormFile imageUpload)
        {
            throw new NotImplementedException();
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
        private ForumListingModel BuildForumListing(Forum forum)
        {

            return new ForumListingModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ForumImageUrl = forum.ImageUrl
            };
        }

    }
}