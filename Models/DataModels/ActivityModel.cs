using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CsharpBlackBelt.Models
{
    public class Activity : DataModel
    {
        [Key]
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description{get;set;}
        public DateTime StartTime{get;set;}
        public DateTime EndTime{get;set;}
        public int Duration{get;set;}
        public string DurationInc{get;set;}
        public int UserId { get; set; }
        public User Creator { get; set; }
        public List<Attandence> Attandences { get; set; }


    }
}