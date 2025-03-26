using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snap.Repository.Data;
using Snap.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snap.APIs.DTOs;

namespace Snap.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly SnapDbContext _context;

        public BlogsController(SnapDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetAllBlogs()
        {
            var blogs = await _context.Blogs
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Blog = b.Blog,
                    UserId = b.UserId
                })
                .ToListAsync();

            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlogById(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(new BlogDto
            {
                Id = blog.Id,
                Blog = blog.Blog,
                UserId = blog.UserId
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetBlogsByUserId(string userId)
        {
            var blogs = await _context.Blogs
                .Where(b => b.UserId == userId)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Blog = b.Blog,
                    UserId = b.UserId
                })
                .ToListAsync();

            return Ok(blogs);
        }

        [HttpPost]
        public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] BlogCreateDto blogDto)
        {
            if (string.IsNullOrEmpty(blogDto.UserId))
            {
                return BadRequest("User ID is required");
            }

            var blog = new Blogs
            {
                Blog = blogDto.Blog,
                UserId = blogDto.UserId
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            var responseDto = new BlogDto
            {
                Id = blog.Id,
                Blog = blog.Blog,
                UserId = blog.UserId
            };

            return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, responseDto);
        }
    }
}
