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
        public IActionResult Get([FromRoute] Guid Id)
        {
            var post = _service.GetPostById(Id);    

            if(post != null) 
              return Ok(post);
              
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest post)
        {

           if (string.IsNullOrEmpty(post.Id))
                post.Id = Guid.NewGuid().ToString();
  
          _service.GetAllPosts().Add(new Post() {Id = Guid.Parse(post.Id), Name = post.Name});

          return Created(GetUriLocationNewItem(post.Id),new PostResponse(){Id = post.Id});

        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute] Guid Id, [FromBody] UpdatePostRequest post)
        {
            var entity = new Post();
            entity.Id = Id;
            entity.Name = post.Name;
            
            if (_service.UpdatePost(entity))
                return Ok();

            return NotFound();

        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute] Guid Id)
        {
            if (_service.DeletePost(Id))
                return NoContent();

            return NotFound();
        }

        private string GetUriLocationNewItem(string id)
        {
           var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
           return baseUrl + ApiRoutes.Posts.Get.Replace("{Id}",id);    
        }
        
    }
}