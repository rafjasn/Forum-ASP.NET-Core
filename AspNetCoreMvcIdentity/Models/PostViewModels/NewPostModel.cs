using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Models.PostViewModels
{
    public class NewPostModel
    {
        public string ForumName { get; set; }
        public string AuthorName { get; set; }
        public string ForumImageUrl { get; set; }
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
