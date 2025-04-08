using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;
using BlogApi.Services.Interfaces;
using BlogApi.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/Post")]
    public class PostsController(IPostService postService, IValidator<PostDto> postValidator) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly IValidator<PostDto> _postValidator = postValidator;

        // Crear una nueva entrada de blog  
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid input data." });

            if (postDto == null)
            {
                return BadRequest(new { Success = false, Message = "El cuerpo de la solicitud está vacío." });
            }

            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { Success = false, Message = "User ID not found in token." });
                }

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest(new { Success = false, Message = "Invalid user ID format." });
                }

                var success = await _postService.CreatePostAsync(postDto, userId);

                if (!success)
                    return StatusCode(500, new { Success = false, Message = "Unable to create the post." });

                return Ok(new { Success = true, Message = $"Post {postDto.Title} created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while creating the post.", Error = ex.Message });
            }
        }

        // Obtener todas las publicaciones del blog  
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (posts, totalCount) = await _postService.GetAllPostsAsync(page, pageSize);

                if (!posts.Any())
                    return NotFound(new { Success = false, Message = "No posts found." });

                return Ok(new
                {
                    Success = true,
                    Data = posts,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while retrieving posts.",
                    Error = ex.Message
                });
            }
        }


        // Obtener una sola publicación de blog  
        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var postDto = await _postService.GetPostByIdAsync(id);
                if (postDto == null)
                    return NotFound(new { Success = false, Message = $"Post with ID {id} not found." });

                return Ok(new { Success = true, Message = "Post retrieved successfully.", Data = postDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving the post.", Error = ex.Message });
            }
        }

        // Eliminar una publicación del blog  
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value);  // 🔐 Se obtiene desde el JWT
                var result = await _postService.DeletePostByIdAsync(id, userId);
                if (!result)
                    return NotFound(new { Success = false, Message = $"Post with ID {id} not found." });

                return Ok(new { Success = true, Message = "Post deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while deleting the Post.", Error = ex.Message });
            }
        }

        // Editar una publicación del blog  
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid input data.", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value);  // 🔐 Se obtiene desde el JWT
                var result = await _postService.UpdatePostAsync(id, postDto, userId);
                if (!result)
                    return NotFound(new { Success = false, Message = $"Post with ID {id} not found." });

                return Ok(new { Success = true, Message = "Post updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while updating the Post.", Error = ex.Message });
            }
        }

        // Filtrar la busqueda de Post
        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<IActionResult> SearchPosts(
        [FromQuery] string? title = null,
        [FromQuery] string? category = null,
        [FromQuery] string? tags = null,
        [FromQuery] string? content = null)
        {
            try
            {
                var filteredPosts = await _postService.FilterPostsAsync(title, category, tags, content);

                if (!filteredPosts.Any())
                    return NotFound(new { Success = false, Message = "No posts found matching the criteria." });

                return Ok(new { Success = true, Message = "Filtered posts retrieved successfully.", Data = filteredPosts });
            }
            catch (ArgumentException ex)  // 🔹 Si la categoría no es válida
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while filtering posts.", Error = ex.Message });
            }
        }




    }
}
