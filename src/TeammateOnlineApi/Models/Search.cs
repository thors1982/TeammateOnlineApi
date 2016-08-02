using System;
using System.Collections.Generic;

namespace TeammateOnlineApi.Models
{
    public class Search
    {
        public List<UserProfile> UserProfiles { get; set; }

        public List<GameAccount> GameAccounts { get; set; }
    }
}
