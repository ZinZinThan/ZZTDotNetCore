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
                return View("AtmIndex");
            }
            return Json(card);
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
        public async Task<IActionResult> AtmList(int id)
        {
            var lst = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return View("AtmList",lst);
        }

        [ActionName("Balance")]
        public async Task<IActionResult> AtmBalance(int id)
        {
            var lst = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return View("AtmBalance", lst);
        }

    }
}
