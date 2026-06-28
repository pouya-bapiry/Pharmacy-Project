namespace Pharmacy.Domain.Dtos.Contact
{
    public class EditAboutUsDto : CreateAboutUsDto
    {
        public long Id { get; set; }
    }

    public enum EditAboutUsResult
    {
        Success, 
        NotFound,
        Error

    }
}
