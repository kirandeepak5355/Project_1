using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs
{
    public class MemberCreateDto
    {
        [Required] public string Name { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        public string Phone {  get; set; }
    }
}
