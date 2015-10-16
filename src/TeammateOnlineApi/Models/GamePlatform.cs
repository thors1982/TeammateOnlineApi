using System.ComponentModel.DataAnnotations;

namespace TeammateOnlineApi.Models
{
    public class GamePlatform
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Url]
        [Display(Name = "URL")]
        public string Url { get; set; }
    }
}
