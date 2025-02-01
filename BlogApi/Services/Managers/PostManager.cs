using AutoMapper;
using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;
using BlogApi.Repository.Interfaces;
using BlogApi.Services.Interfaces;

namespace BlogApi.Services.Managers
{
    public class PostManager(IPostRepository repository, IMapper mapper) : IPostService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPostRepository _repository = repository;


        public async Task<bool> CreatePostAsync(PostDto postDto)
        {
            if (postDto == null) throw new ArgumentNullException(nameof(postDto));

            var post = _mapper.Map<Post>(postDto);
            return await _repository.AddPostAsync(post);
        }

        public async Task<bool> DeletePostById(int id)
        {
            return await _repository.DeletePostAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _repository.GetAllPostsAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _repository.GetPostByIdAsync(id);
        }
    }
}
