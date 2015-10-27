using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeammateOnlineApi.ViewModels
{
    public class GameAccountViewModel
    {
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int GamePlatformId { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
