using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcIdentity.Models;
using AspNetCoreMvcIdentity.Models.ForumViewModels;
using AspNetCoreMvcIdentity.Models.PostViewModels;
using AspNetCoreMvcIdentity.Models.SearchViewModels;
using AspNetCoreMvcIdentity.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcIdentity.Controllers
{
    public class SearchController : Controller
    {
        private readonly IForum _forum;
        private readonly IPost _post;

        public SearchController(IForum forum, IPost post)
        {
            _forum = forum;
            _post = post;
        }

        public IActionResult Results(string searchQuery)
        {

            var posts = _post.GetFilteredPosts(searchQuery);


            var results = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());

            var postListings = posts.Select(p => new PostListingModel
            {
                Id = p.Id,
                AuthorId = p.User.Id.ToString(),
                AuthorName = p.User.UserName,
                AuthorRating = p.User.Rating,

                Title = p.Title,
                DatePosted = p.Created.ToString(),
                RepliesCount = p.Replies.Count(),
                Forum = BuildForumListing(p)
            });

            var model = new SearchResultModel
            {
                Posts = postListings,
                SearchQuery = searchQuery,
                EmptySearchResults = results
            };


            return View(model);
        }

        private ForumListingModel BuildForumListing(Post p)
        {

            var forum = p.Forum;
            return new ForumListingModel
            {
                Id = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                Title = forum.Title,
                Description = forum.Description
            };
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }
    }
}