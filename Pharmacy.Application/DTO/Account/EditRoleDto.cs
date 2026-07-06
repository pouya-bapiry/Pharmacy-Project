namespace Pharmacy.Application.DTO.Account
{
    public class EditRoleDto : CreateRoleDto
    {
        public long Id { get; set; }
    }

    public enum EditRoleResult
    {
        Success,
        Error
    }
}
