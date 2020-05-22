using System;
using System.Collections.Generic;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
         List<Post> GetAllPosts();

         Post GetPostById(Guid Id);

         bool UpdatePost(Post post);

         bool DeletePost(Guid Id);
    }
}