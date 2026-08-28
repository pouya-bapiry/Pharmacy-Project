using Pharmacy.Application.DTO.Paging;


namespace Pharmacy.Application.DTO.Blog
{
    public class FilterBlogDto : BasePaging
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string BlogCategory { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string Image { get; set; }
        public List<Domain.Entities.Blog.Blog> Blogs { get; set; }


        #region Methods


        public FilterBlogDto SetBlog(List<Domain.Entities.Blog.Blog> blogs)
        {
            this.Blogs = blogs;
            return this;
        }

        public FilterBlogDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }
    }

        #endregion
    
}
