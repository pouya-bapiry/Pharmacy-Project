using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Blog;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.Web.Controllers
{
    public class BlogController : SiteBaseController
    {
        #region Fields and ctor

        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #endregion

        #region Actions
        [HttpGet("blog-list")]
        public async Task<IActionResult> BlogsList(FilterBlogDto filter)
        {
            var blog = await _blogService.FilterBlogs(filter);
            return View(blog);
        }

        [HttpGet("blog-detail/{blogId}")]
        public async Task<IActionResult> BlogDetail(long blogId)
        {
            var blog = await _blogService.GetBlogDetail(blogId);
            return View(blog);
        }

        #endregion

    }
}
