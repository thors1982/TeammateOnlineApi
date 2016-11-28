using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeammateOnlineApi.Models
{
    public class UserGroup : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public string Role { get; set; }

        public virtual Group Group { get; set; }
    }
}
