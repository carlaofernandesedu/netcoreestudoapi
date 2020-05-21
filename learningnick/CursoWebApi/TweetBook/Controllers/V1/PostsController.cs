using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Domain;

namespace TweetBook.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> _posts;

        public PostsController()
        {
            Seed();    
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);    
        }

        private void Seed()
        {
            _posts = new List<Post>();
            
            for(int i =0; i < 3; i++)
            {
                var newGuid = System.Guid.NewGuid().ToString();
                _posts.Add(new Post(){Id = newGuid });
            }
        }
    }
}