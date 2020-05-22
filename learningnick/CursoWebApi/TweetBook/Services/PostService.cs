using System;
using System.Collections.Generic;
using TweetBook.Domain;
using System.Linq;
using TweetBook.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _repository;

        public PostService(DataContext repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _repository.Posts.AddAsync(post);
            var result = await _repository.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeletePostAsync(Guid Id)
        {
            var post = await GetPostByIdAsync(Id);
            _repository.Posts.Remove(post);
            var result = await _repository.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _repository.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid Id)
        {
            return await _repository.Posts.SingleOrDefaultAsync(x=> x.Id == Id);
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            _repository.Posts.Update(post);
            var result = await _repository.SaveChangesAsync();
            return result > 0;
        }

        
    }
}