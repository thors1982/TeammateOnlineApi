using System.ComponentModel.DataAnnotations;

namespace TeammateOnlineApi.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    }
}
