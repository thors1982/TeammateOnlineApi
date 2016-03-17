using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeammateOnlineApi.Models
{
    public class Identity
    {
        public int UserProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
