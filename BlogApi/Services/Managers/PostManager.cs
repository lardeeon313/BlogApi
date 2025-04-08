using AutoMapper;
using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Repository.Interfaces;
using BlogApi.Repository.Repositories;
using BlogApi.Services.Interfaces;

namespace BlogApi.Services.Managers
{
    public class PostManager(IPostRepository repository, IMapper mapper) : IPostService
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IPostRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));


        public async Task<bool> CreatePostAsync(PostDto postDto, int userId)
        {
            if (postDto == null) throw new ArgumentNullException(nameof(postDto));

            // Mapear el DTO a la entidad Post
            var post = _mapper.Map<Post>(postDto);

            // Asignar el UserId (asociación entre Post y User)
            post.UserId = userId;

            // Agregar el post a la base de datos
            await _repository.AddPostAsync(post);
            return true;
        }

        public async Task<bool> DeletePostByIdAsync(int id, int userId)
        {
            var post = await _repository.GetPostByIdAsync(id);
            if (post == null)
                return false;

            // Verificar que el post pertenece al usuario que lo está eliminando
            if (post.UserId != userId)
                return false;

            await _repository.DeletePostAsync(id);
            return true;
        }

        public async Task<(IEnumerable<PostDto> Posts, int TotalCount)> GetAllPostsAsync(int page, int pageSize)
        {
            // Obtiene todos los posts desde la base de datos
            var posts = await _repository.GetAllPostsAsync();

            // Calcula el total de posts (para la paginación)
            int totalCount = posts.Count();

            // Aplica la paginación: Skip y Take
            var paginatedPosts = posts
                .Skip((page - 1) * pageSize) // Salta los registros previos según la página
                .Take(pageSize) // Limita los resultados a pageSize
                .ToList();

            // Mapea los posts obtenidos a PostDto
            var mappedPosts = _mapper.Map<List<PostDto>>(paginatedPosts);

            // Retorna los posts mapeados junto con el total de registros
            return (mappedPosts, totalCount);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await _repository.GetPostByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            return _mapper.Map<PostDto>(post);
        }

        public async Task<bool> UpdatePostAsync(int id, PostDto postDto, int userId)
        {
            if (postDto == null)
                throw new ArgumentNullException(nameof(postDto));

            var existingPost = await _repository.GetPostByIdAsync(id);
            if (existingPost == null)
                return false;

            // Verificar que el usuario que está intentando actualizar el post sea el mismo que lo creó
            if (existingPost.UserId != userId)
                return false;

            _mapper.Map(postDto, existingPost);
            await _repository.UpdatePostAsync(existingPost);
            return true;
        }


        public async Task<IEnumerable<PostDto>> FilterPostsAsync(string? title, string? category, string? tags, string? content)
        {
            Category? categoryEnum = null;

            if (!string.IsNullOrEmpty(category))
            {
                if (Enum.TryParse(category, true, out Category parsedCategory))
                {
                    categoryEnum = parsedCategory;
                }
                else
                {
                    throw new ArgumentException($"Invalid category value: {category}");
                }
            }

            var posts = await _repository.FilterPostsAsync(title, categoryEnum, tags, content);

            return posts.Select(p => new PostDto
            {
                Title = p.Title,
                Category = p.Category, // Convertimos de enum a string para el DTO
                Tags = p.Tags.Split(',').ToList(),
                Content = p.Content,
            }).ToList();
        }
    }
}
