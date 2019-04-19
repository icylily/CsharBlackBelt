using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CsharpBlackBelt.Models
{
    public class DashboardModel
    {
        public User Currentuser;
        public List<Activity> JoinedAct;
        public List<Activity> AllAct;

    }

}