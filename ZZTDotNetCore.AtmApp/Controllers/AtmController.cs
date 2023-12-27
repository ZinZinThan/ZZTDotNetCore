using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> AtmLogin(AtmDataModel reqModel)
        {
            var card = await _context.AtmDatas.FirstOrDefaultAsync(x => x.CardNumber == reqModel.CardNumber && x.Pin == reqModel.Pin);

            if (card is null)
            {
                TempData["Message"] = "Card not found.";
                TempData["IsSuccess"] = false;
                return Redirect("/atm");
            }
            return Redirect("/atm/list");
        }

        [ActionName("Create")]
        public IActionResult AtmCreate()
        {
            return View("AtmCreate");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> AtmSave(AtmDataModel reqModel)
        {
            if( reqModel.Name == null || reqModel.CardNumber == 0 || reqModel.Pin == 0 || reqModel.Balance == 0)
            {
                TempData["Message"] = " Please fill all field.";
                TempData["IsSuccess"] = false;
                return View("AtmCreate");
            }

            await _context.AtmDatas.AddAsync(reqModel);
            int result = await _context.SaveChangesAsync();

            TempData["Message"] = result > 0 ? "Register Successful." : "Register Failed.";
            TempData["IsSuccess"] = result > 0;
          
            return View("AtmCreate");
        }

        [ActionName("List")]
        public IActionResult AtmList()
        {
            return View("AtmList");
        }

    }
}
