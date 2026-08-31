using Microsoft.AspNetCore.Http;
using Pharmacy.Application.DTO.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IBlogService : IAsyncDisposable
    {
        Task<FilterBlogDto> FilterBlogs(FilterBlogDto filter);
        Task<CreateBlogResult> CreateBlog(CreateBlogDto create, IFormFile image);
        Task<EditBlogDto> GetBlogForEdit(long id);
        Task<EditBlogResult> EditBlog(EditBlogDto edit, IFormFile? image);
        Task<FilterBlogDto> GetBlogDetail(long id);
    }
}
