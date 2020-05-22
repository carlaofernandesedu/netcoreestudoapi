using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync();

        Task<Post> GetPostByIdAsync(Guid Id);

        Task<bool> UpdatePostAsync(Post post);

        Task<bool> CreatePostAsync(Post post);

         Task<bool> DeletePostAsync(Guid Id);
    }
}