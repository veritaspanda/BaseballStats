using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseballStats.Models
{
    public class PlayerSearchInputModel
    {
        public string PlayerFirstName { get; set; }

        [Required]
        public string PlayerLastName { get; set; }
    }
}