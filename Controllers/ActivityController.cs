using System;
using System.Collections.Generic;
// using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsharpBlackBelt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace CsharpBlackBelt.Controllers
{
    public class ActivityController : Controller
    {
        private ActivityContext dbContext;
        public ActivityController(ActivityContext context)
        {
            dbContext = context;
        }

        [HttpGet("/new")]
        public IActionResult Index()
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Need Register or login first!";
                return View("Warning");
            }
            
            return View("NewActivity");
        }

        [HttpPost("/NewActivity")]
        public IActionResult NewActivity(NewActivity addActivity )
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Session time out. Need Register or login first!";
                return View("Warning");
            }

            if (!ModelState.IsValid)
            {
                return View("NewActivity");
            }

            DateTime startTime = addActivity.GetStartTime();
            DateTime now = DateTime.Now;
            if (DateTime.Compare(startTime, now) < 0)
            {
                ModelState.AddModelError("StartDate", "The Activity  must be start in the future!");
                return View("NewActivity");
            }

            Activity NewActivity = GetNewActivity(addActivity);
            dbContext.Activitys.Add(NewActivity);
            dbContext.SaveChanges();

            return Redirect("/dashboard");
        }

        public Activity GetNewActivity(NewActivity addActivity)
        {
            Activity NewActivity = new Activity();
            NewActivity.Title = addActivity.Title;
            NewActivity.Description = addActivity.Description;
            NewActivity.Duration = addActivity.Duration;
            NewActivity.DurationInc = addActivity.DurationInc;
            NewActivity.StartTime = addActivity.GetStartTime();
            NewActivity.EndTime = addActivity.GetEndTime(NewActivity.StartTime);
            NewActivity.UserId = Loginuser.GetUserID(HttpContext);

            return NewActivity;
        }

        [HttpGet("/ViewActivity/{activityId}")]
        public IActionResult ViewActivity(int activityId)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Session time out. Need Register or login first!";
                return View("Warning");
            }
            var viewActivity = dbContext.Activitys
            .Include(act => act.Creator)
            .Include(act => act.Attandences)
            .ThenInclude(att => att.User)
            .FirstOrDefault(act => act.ActivityId == activityId);
            if(viewActivity.UserId == UserId)
            {
                ViewBag.join = "owner";
            }
            if(dbContext.Attandences.Where(att =>((att.UserId == UserId) && (att.ActivityId == activityId))).ToList().Count()==0)
            {
                ViewBag.join = "joinale";
            }
            else
            {
                ViewBag.join = "notjoinale";
            }
            ViewBag.UserId = UserId;
            return View(viewActivity);
        }

        [HttpGet("/activity/delete/{activityId}")]
        public IActionResult DeleteActivity(int activityId)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Session time out. Need Register or login first!";
                return View("Warning");
            }
            Activity thisAct = dbContext.Activitys.FirstOrDefault(act => act.ActivityId == activityId);
            if (thisAct.UserId != UserId)
            {
                ViewBag.message = "Can not delete a weeding  that not created by yourself!";
                return View("Warning");
            }
            dbContext.Activitys.Remove(thisAct);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpGet("/activity/join/{activityId}/{userId}")]
        public IActionResult RSVP(int activityId, int userId)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Session time out. Need Register or login first!";
                return View("Warning");
            }
            if (userId != UserId)
            {
                ViewBag.message = "Can not  Join for other user!";
                return View("Warning");
            }
            Activity thisAct = dbContext.Activitys.FirstOrDefault(act => act.ActivityId == activityId);
            List<Activity> joinedAct = dbContext.Activitys
            .Include(act => act.Attandences)
            .Where(act => act.Attandences.Any(user => user.UserId == userId))
            .ToList();

            foreach(Activity activity in joinedAct)
            {
                // if (DateTime.Compare(startTime, now) < 0)
                if((DateTime.Compare(thisAct.StartTime,activity.EndTime)<=0)&&(DateTime.Compare(thisAct.StartTime, activity.StartTime) < 0))
                {
                    ViewBag.message = "Time conflict!";
                    return View("Warning");
                }
                if ((DateTime.Compare(thisAct.EndTime, activity.EndTime) <= 0) && (DateTime.Compare(thisAct.StartTime, activity.StartTime) >=0))
                {
                    ViewBag.message = "Time conflict!";
                    return View("Warning");
                }

                // if ((DateTime.Compare(thisAct.StartTime, activity.EndTime) < 0) && (DateTime.Compare(thisAct.StartTime, activity.StartTime) > 0))
                // {
                //     ViewBag.message = "Time conflict!";
                //     return View("Warning");
                // }
                // if ((DateTime.Compare(thisAct.StartTime, activity.EndTime) < 0) && (DateTime.Compare(thisAct.StartTime, activity.StartTime) > 0))
                // {
                //     ViewBag.message = "Time conflict!";
                //     return View("Warning");
                // }
                

                // Console.WriteLine("activity end", activity.EndTime.ToString());
                // Console.WriteLine("this star", thisAct.StartTime.ToString());
                // Console.WriteLine("(DateTime.Compare(thisAct.StartTime,activity.EndTime)<0)",(DateTime.Compare(thisAct.StartTime, activity.EndTime) < 0).ToString());

            }

            
            Attandence newAtten = new Attandence();
            newAtten.UserId = userId;
            newAtten.ActivityId = activityId;
            dbContext.Attandences.Add(newAtten);
            dbContext.SaveChanges();

            return Redirect("/dashboard");
        }


        [HttpGet("/activity/leave/{activityId}/{userId}")]
        public IActionResult UnRSVP(int activityId, int userId)
        {
            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Session time out. Need Register or login first!";
                return View("Warning");
            }
            if (userId != UserId)
            {
                ViewBag.message = "Can not  Un-RSVP for other user!";
                return View("Warning");
            }
            Attandence delAtten = dbContext.Attandences
            .Where(att => ((att.ActivityId == activityId) && (att.UserId == userId)))
            .FirstOrDefault();
            dbContext.Attandences.Remove(delAtten);
            dbContext.SaveChanges();

            return Redirect("/dashboard");
        }
    }

}   