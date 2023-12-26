using Microsoft.AspNetCore.Mvc;

namespace ZZTDotNetCore.AtmApp.Controllers
{
    public class AtmController : Controller
    {
        [ActionName("Index")]
        public IActionResult AtmIndex()
        {
            return View("AtmIndex");
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult AtmLogin()
        {
            return Redirect("/atm/list");
        }

        [ActionName("Create")]
        public IActionResult AtmCreate()
        {
            return View("AtmCreate");
        }

        [HttpPost]
        [ActionName("Save")]
        public IActionResult AtmSave()
        {
            return Redirect("/atm/index");
        }

        [ActionName("List")]
        public IActionResult AtmList()
        {
            return View("AtmList");
        }
    }
}
