using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CsharpBlackBelt.Models
{
    public class Attandence : DataModel
    {
        [Key]
        public int AttandenceId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ActivityId { get; set; }
        public User User { get; set; }
        public Activity Activity { get; set; }
    }

}