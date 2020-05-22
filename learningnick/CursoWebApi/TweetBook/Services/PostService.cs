using System;
using System.Collections.Generic;
using TweetBook.Domain;
using System.Linq;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private List<Post> _posts;

        public PostService()
        {
            Seed();
        }
        public List<Post> GetAllPosts()
        {
            return _posts;
        }

        public Post GetPostById(Guid Id)
        {
            return _posts.SingleOrDefault(x=> x.Id == Id);
        }

        private void Seed()
        {
            _posts = new List<Post>();
            
            for(int i =0; i < 3; i++)
            {
                var newGuid = System.Guid.NewGuid().ToString();
                _posts.Add(new Post(){Id = Guid.Parse(newGuid), Name = $"Post Name:{i}"});
            }
        }
    }
}