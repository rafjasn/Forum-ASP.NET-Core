using AspNetCoreMvcIdentity.Models.ForumViewModels;
using AspNetCoreMvcIdentity.Models.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Models.HomeViewModels
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
        public IEnumerable<PostListingModel> PopularPosts { get; set; }
        public IEnumerable<ForumListingModel> Forums { get; set; }
    }
}
