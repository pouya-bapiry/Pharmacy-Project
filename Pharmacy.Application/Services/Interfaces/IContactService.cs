using Pharmacy.Application.DTO.Contact;
using Pharmacy.Application.DTO.Site;
using Pharmacy.Domain.Dtos.Contact;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        Task<List<AboutUsDto>> GetAll();
        Task<FilterContactUs> FilterContactUs(FilterContactUs filter);
        Task<CreateAboutUsResult> CreateAboutUs(CreateAboutUsDto about);
        Task<EditAboutUsDto> GetAboutUsForEdit(long id);
        Task<EditAboutUsResult> EditAboutUs(EditAboutUsDto edit, string username);
        Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId);


      

    }
}
