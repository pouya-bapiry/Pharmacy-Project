using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Blog;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.Web.Areas.Administration.Controllers
{
    public class BlogController : AdminBaseController
    {
        #region Fields and ctor

        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #endregion

        #region Actions

        #region Filter
        [HttpGet("blog-list")]
        public async Task<IActionResult> FilterBlogs(FilterBlogDto filter)
        {

            var blog = await _blogService.FilterBlogs(filter);
            return View(blog);
        }
        #endregion

        #region Create
        [HttpGet("create-blog")]
        public async Task<IActionResult> CreateBlog()
        {

            return View();

        }

        [HttpPost("create-blog")]
        public async Task<IActionResult> CreateBlog(CreateBlogDto createBlog, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var blog = await _blogService.CreateBlog(createBlog, image);
                switch (blog)
                {
                    case CreateBlogResult.Error:
                        TempData[ErrorMessage] = "در افزودن اطلاعات خطایی رخ داد";
                        break;
                    case CreateBlogResult.Success:
                        TempData[SuccessMessage] = "افزودن دسته مقاله با موفقیت انجام شد";
                        return RedirectToAction("FilterBlogs", "Blog");
                }
            }
            return View();
        }
        #endregion

        #region Edit

        #endregion

        #endregion
    }
}
