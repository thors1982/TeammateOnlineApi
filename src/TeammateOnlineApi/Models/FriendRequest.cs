using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeammateOnlineApi.Models
{
    public class FriendRequest : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int FriendUserProfileId { get; set; }

        public string Note { get; set; }

        public bool IsPending { get; set; }

        public bool IsAccepted { get; set; }

        [NotMapped]
        public bool IsIncomingRequest { get; set; } 

        public virtual UserProfile UserProfile { get; set; }

        public virtual UserProfile FriendUserProfile { get; set; }
    }
}
