using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using ZZTDotNetCore.AtmApp.EFDbContext;
using ZZTDotNetCore.AtmApp.Models;

namespace ZZTDotNetCore.AtmApp.Controllers
{
    public class AtmController : Controller
    {
		private readonly AppDbContext _context;

		public AtmController(AppDbContext context)
		{
			_context = context;
		}

		[ActionName("Index")]
        public IActionResult AtmIndex()
        {
            return View("AtmIndex");
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult AtmLogin(AtmDataModel reqModel)
        {
            var card = _context.AtmDatas.FirstOrDefault(x => x.CardNumber == reqModel.CardNumber && x.Pin == reqModel.Pin);

            if (card is null)
            {
                TempData["Message"] = "Card not found.";
                TempData["IsSuccess"] = false;
                return Redirect("/atm");
            }
            return Redirect("/atm/list");
        }

        [ActionName("List")]
        public IActionResult AtmList()
        {
            return View("AtmList");
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

       
    }
}
