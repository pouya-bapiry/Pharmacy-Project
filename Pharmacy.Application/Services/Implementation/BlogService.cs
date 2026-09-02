using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Blog;
using Pharmacy.Application.DTO.Paging;
using Pharmacy.Application.Extensions;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.Entities.Blog;
using Pharmacy.Domain.IRepository;

namespace Pharmacy.Application.Services.Implementation
{
    public class BlogService : IBlogService
    {
        #region Fields and Ctor

        private readonly IGenericRepository<Blog> _blogRepository;

        public BlogService(IGenericRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }




        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if (_blogRepository != null)
            {
                await _blogRepository.DisposeAsync();
            }
        }


        #endregion

        #region Methods

        #region Get

        public async Task<FilterBlogDto> FilterBlogs(FilterBlogDto filter)
        {

            var query = _blogRepository
                .GetQuery()
                .AsQueryable();
            #region Filter

           
            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));
            }

            #endregion

            #region Paging


            var articleCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, articleCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetBlog(allEntities);

        }

        public async Task<FilterBlogDto> GetBlogDetail(long id)
        {
            var blog = await _blogRepository.GetQuery().AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            return new FilterBlogDto
            {
                Title = blog.Title,
                ShortDescription = blog.ShortDescription,
                Description = blog.Description,
                BlogCategory=blog.BlogCategory,
                LastUpdateDate=blog.LastUpdateDate.ToStringShamsiDate(),
                Image = blog.Image
            };
        }
        #endregion

        #region Create
        public async Task<CreateBlogResult> CreateBlog(CreateBlogDto create, IFormFile image)
        {
            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtension.ArticleOriginServer,
                    100, 100, PathExtension.ProductThumbServer);

                var blog = new Blog
                {
                    Title = create.Title,
                    BlogCategory = create.BlogCategory,
                    Description = create.Description,
                    Image = imageName,
                    ShortDescription = create.ShortDescription,

                };
                await _blogRepository.AddEntity(blog);
                await _blogRepository.SaveChanges();
                return CreateBlogResult.Success;
            }
            return CreateBlogResult.Error;
        }
        #endregion

        #region Edit
        public async Task<EditBlogDto> GetBlogForEdit(long id)
        {

            var blog = await _blogRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

            return new EditBlogDto
            {
                Id = id,
                BlogCategory = blog.BlogCategory,
                Description = blog.Description,
                ShortDescription = blog.ShortDescription,
                Image = blog.Image,
                Title = blog.Title,
            };
        }
        public async Task<EditBlogResult> EditBlog(EditBlogDto edit, IFormFile? image)
        {
            var blog = await _blogRepository.GetQuery().AsQueryable().FirstOrDefaultAsync(x => x.Id == edit.Id);

            if (blog == null)
            {

                return EditBlogResult.Error;

            }

            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtension.ArticleOriginServer,
                    100, 100, PathExtension.ProductThumbServer);

                blog.Image = imageName;
            }
            blog.Title= edit.Title;
            blog.Description= edit.Description;
            blog.ShortDescription= edit.ShortDescription;
            blog.BlogCategory = edit.BlogCategory;
           
             _blogRepository.EditEntity(blog);
            await _blogRepository.SaveChanges();
            return EditBlogResult.Success;
        }




        #endregion


        #endregion

    }
}
