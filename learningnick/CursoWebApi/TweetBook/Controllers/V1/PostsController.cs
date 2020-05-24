using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize] 
    public class PostsController : Controller
    {
        private readonly IPostService _service;

       
       public PostsController(IPostService service)
       {
           _service = service;
       }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllPostsAsync());    
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid Id)
        {
            var post = await _service.GetPostByIdAsync(Id);    

            if(post != null) 
              return Ok(post);
              
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePostRequest post)
        {

          var entidade = new Post() { Name = post.Name};
          await _service.CreatePostAsync(entidade);

          var retorno = new PostResponse(){Id = entidade.Id.ToString()};
          return Created(GetUriLocationNewItem(entidade.Id.ToString()),retorno);

        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid Id, [FromBody] UpdatePostRequest post)
        {
            var entity = new Post();
            entity.Id = Id;
            entity.Name = post.Name;
            
            if (await _service.UpdatePostAsync(entity))
                return Ok();

            return NotFound();

        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            if (await _service.DeletePostAsync(Id))
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