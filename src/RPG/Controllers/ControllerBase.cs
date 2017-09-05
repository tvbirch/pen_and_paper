using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.Context;

namespace RPG.Controllers
{
    public class ControllerBase : Controller
    {
        public ContextManager Context;
        public ControllerBase()
        {
            Context = new ContextManager();
        }
    }
}