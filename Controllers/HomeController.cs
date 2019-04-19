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
    public class HomeController : Controller
    {
        private ActivityContext dbContext;
        public HomeController(ActivityContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(NewUser newUser)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                if (dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<NewUser> Hasher = new PasswordHasher<NewUser>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    User addUser = newUser.GetNewuser();
                    dbContext.Users.Add(addUser);
                    dbContext.SaveChanges();
                    Loginuser.SetLogin(HttpContext, addUser.UserId);
                    return Redirect("Dashboard");
                }
            }
        }


        [HttpPost("Login")]
        public IActionResult Login(Loginuser newLoginUser)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                User needLogin = dbContext.Users.FirstOrDefault(u => u.Email == newLoginUser.LogEmail);
                if (needLogin == null)
                {
                    ModelState.AddModelError("LogEmail", "This email didn't exist.Please rigester first!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                var verifyPass = Hasher.VerifyHashedPassword(needLogin, needLogin.Password, newLoginUser.LogPassword);
                if (verifyPass == 0)
                {
                    ModelState.AddModelError("LogPassword", "Password is wrong!");
                    return View("Index");
                }
                else
                {
                    Loginuser.SetLogin(HttpContext, needLogin.UserId);
                    return Redirect("Dashboard");
                }
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }


        ////////////////////////////////////////////////////////
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {

            int UserId = Loginuser.GetUserID(HttpContext);
            if (UserId == 0)
            {
                ViewBag.message = " Need Register or login first!";
                return View("Warning");
            }
            DashboardModel Dashboard = SetupDashboard(UserId);
            return View(Dashboard);
        }

        public DashboardModel SetupDashboard(int userId)
        {
            DashboardModel DashboardModel = new DashboardModel();

            DashboardModel.Currentuser = dbContext.Users.FirstOrDefault(user => user.UserId == userId);

            List<Activity> allActivity = dbContext.Activitys
            .Include(act => act.Creator)
            .Include(act => act.Attandences)
            .ThenInclude(attandence => attandence.User)
            .OrderByDescending(act => act.StartTime)
            .ToList();

            DashboardModel.AllAct = allActivity;

            List<Activity> joinedAct = dbContext.Activitys
            .Include(act => act.Attandences)
            .Where(act => act.Attandences.Any(user => user.UserId == userId))
            .ToList();

            DashboardModel.JoinedAct = joinedAct;
            return DashboardModel;
        }


    }
}
