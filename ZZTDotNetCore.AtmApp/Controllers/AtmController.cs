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

                MessageModel model1 = new MessageModel(false, "Login Failed.");
                return Json(model1);
            }

            MessageModel model = new MessageModel(true, "Successful Login");
            return Json(model);
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
            await _context.AtmDatas.AddAsync(reqModel);
            int result = await _context.SaveChangesAsync();

            string message = result > 0 ? "Register Successful." : "Register Failed.";
          
            MessageModel model = new MessageModel(result > 0, message);
            return Json(model);
        }

        [ActionName("List")]
        public IActionResult AtmList()
        {
            return View("AtmList");
        }


    }
}
