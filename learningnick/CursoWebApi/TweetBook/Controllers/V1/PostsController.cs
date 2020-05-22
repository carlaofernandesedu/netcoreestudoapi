using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class PostsController : Controller
    {
        private readonly IPostService _service;

       public PostsController(IPostService service)
       {
           _service = service;
       }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllPosts());    
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get( Guid Id)
        {
            var post = _service.GetPostById(Id);    

            if(post != null) 
              return Ok(post);
              
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest post)
        {
            string newId = string.Empty;

           if (string.IsNullOrEmpty(post.Id))
           {
                newId = Guid.NewGuid().ToString();
                post.Id = newId;
           }
          _service.GetAllPosts().Add(new Post() {Id = Guid.Parse(post.Id), Name = post.Name});

          return Created(GetUriLocationNewItem(newId),new PostResponse(){Id = post.Id});

        }

        public IActionResult Update()
        {
            throw new NotImplementedException();
        }

        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }

        private string GetUriLocationNewItem(string id)
        {
           var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
           return baseUrl + ApiRoutes.Posts.Get.Replace("{Id}",id);    
        }
        
    }
}