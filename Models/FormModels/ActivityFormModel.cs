using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CsharpBlackBelt.Models
{
    public class NewActivity
    {
        [Required(ErrorMessage = "Title is required!")]
        [MinLength(3, ErrorMessage = "Title Must be 3 characters or longer!")]
        [Display(Name = "Title:")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        [MinLength(3, ErrorMessage = "Description Must be 3 characters or longer!")]
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Date is required!")]
        [Display(Name = "Date:")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Time is required!")]
        [Display(Name = "Time:")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Duration is required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Duration must greatter then 0!")]
        [Display(Name = "Duration:")]
        public int Duration { get; set; }=0;
        public string DurationInc { get; set; }

        public DateTime GetStartTime()
        {
            DateTime startTime = new DateTime();
            startTime = StartDate.Date.Add(StartTime.TimeOfDay);
            return startTime;
        }

        public DateTime GetEndTime(DateTime startTime)
        {
            DateTime endTime = new DateTime();
            if (DurationInc == "minutes")
            {
                endTime = startTime.AddMinutes(Duration);
            }
            if (DurationInc == "hours")
            {
                endTime = startTime.AddHours(Duration);
            }
            if (DurationInc == "days")
            {
                endTime = startTime.AddDays(Duration);
            }
            return endTime;
        }
        

    }


}
