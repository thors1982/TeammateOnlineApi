using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeammateOnlineApi.Models
{
    public class Friend : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int FriendUserProfileId { get; set; }

        public virtual UserProfile FriendUserProfile { get; set; }
    }
}
