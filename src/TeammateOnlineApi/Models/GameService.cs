using System.ComponentModel.DataAnnotations;

namespace TeammateOnlineApi.Models
{
    public class GameService
    {
        [Key]
        public int GameServiceId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Url]
        [Display(Name = "URL")]
        public string Url { get; set; }
    }
}
