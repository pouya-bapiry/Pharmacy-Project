using Eshop.Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Contact;
using Pharmacy.Application.DTO.Site;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Dtos.Contact;
using Pharmacy.Domain.Entities.Contact;
using Pharmacy.Domain.Entities.Site;
using Pharmacy.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Implementation
{
    public class ContactService : IContactService
    {

        #region Fields and Ctor

        private readonly IGenericRepository<ContactUs> _contactRepository;
        private readonly IGenericRepository<AboutUs> _aboutUsRepository;

        public ContactService(IGenericRepository<ContactUs> contactRepository,
            IGenericRepository<AboutUs> aboutUsRepository)
        {
            _contactRepository = contactRepository;
            _aboutUsRepository = aboutUsRepository;
        }







        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_contactRepository != null)
            {
                await _contactRepository.DisposeAsync();
            }
            if (_aboutUsRepository != null)
            {
                await _aboutUsRepository.DisposeAsync();
            }
        }



        #endregion
        #region Methods
        #region AboutUs

        public async Task<List<AboutUsDto>> GetAll()
        {
            return await _aboutUsRepository.GetQuery().AsQueryable().Select(x => new AboutUsDto
            {
                Id = x.Id,
                HeaderTitle = x.HeaderTitle,
                Description = x.Description,
                CreateDate = x.CreateDate.ToStringShamsiDate(),
                LastUpdateDate = x.LastUpdateDate.ToStringShamsiDate()

            }).ToListAsync();
        }
        #region Create
        public async Task<CreateAboutUsResult> CreateAboutUs(CreateAboutUsDto about)
        {
            var newAboutUs = new AboutUs()
            {
                Description = about.Description,
                HeaderTitle = about.HeaderTitle,
            };
            _aboutUsRepository.AddEntity(newAboutUs);
            _aboutUsRepository.SaveChanges();

            return CreateAboutUsResult.Success;

        }
        #endregion
        #region Edit
        public async Task<EditAboutUsDto> GetAboutUsForEdit(long id)
        {
            var about = await _aboutUsRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return null;
            }
            return new EditAboutUsDto
            {
                Id = about.Id,
                HeaderTitle = about.HeaderTitle,
                Description = about.Description,

            };
        }

        public async Task<EditAboutUsResult> EditAboutUs(EditAboutUsDto edit, string username)
        {
            var about = await _aboutUsRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (about == null)
            {
                return EditAboutUsResult.NotFound;

            }
            about.HeaderTitle = edit.HeaderTitle;
            about.Description = edit.Description;

            _aboutUsRepository.EditEntityByUser(about, username);

            _aboutUsRepository.SaveChanges();

            return EditAboutUsResult.Success;

        }
        #endregion





        #endregion

        #region ContactUs

        public async Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId)
        {
            // todo : Use Sanitizer to sanitize input data

            var newContact = new ContactUs
            {
                UserId = (userId != null && userId.Value != 0) ? userId.Value : (long?)null,
                UserIp = userIp,
                Email = contact.Email,
                Fullname = contact.Fullname,
                MessageSubject = contact.MessageSubject,
                MessageText = contact.MessageText,
            };

            await _contactRepository.AddEntity(newContact);
            await _contactRepository.SaveChanges();
        }


        public Task<FilterContactUs> FilterContactUs(FilterContactUs filter)
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion

    }
}
