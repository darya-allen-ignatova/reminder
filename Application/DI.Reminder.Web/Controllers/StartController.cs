﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DI.Reminder.Web.Controllers
{
    public class StartController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }
    }
}