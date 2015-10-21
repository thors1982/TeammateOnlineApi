using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeammateOnlineApi.Models
{
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
