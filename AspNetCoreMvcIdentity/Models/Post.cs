using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcIdentity.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual Forum Forum { get; set; }
        public virtual IEnumerable<PostReply> Replies { get; set; }


    }
}
 