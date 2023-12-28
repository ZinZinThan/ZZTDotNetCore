using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
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
                TempData["Message"] = "Card not recognized. Please try again!";
                TempData["IsSuccess"] = false;
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
            var card = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            return View("AtmList",card);
        }

        [ActionName("Balance")]
        public async Task<IActionResult> AtmBalance(int id)
        {
            var card = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
           
            return View("AtmBalance", card);
        }

        [ActionName("Deposit")]
        public async Task<IActionResult> AtmDeposit(int id)
        {
            var card = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return View("AtmDeposit",card);
        }

        [HttpPost]
        [ActionName("Deposit")]
        public async Task<IActionResult> AtmDeposit(int id, AtmDataModel reqModel)
        {
            var card = await _context.AtmDatas.FirstOrDefaultAsync(x => x.Id == id);

            card.Balance += reqModel.Balance;
            _context.AtmDatas.Entry(card).State = EntityState.Modified;
            int result = _context.SaveChanges();
            string message = result > 0 ? "Deposit Successful." : "Deposit Failed.";

            MessageModel model = new MessageModel(result > 0, message);
            return Json(model);
        }

        [ActionName("Withdraw")]
        public async Task<IActionResult> AtmWithdraw(int id)
        {
            var card = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return View("AtmWithdraw",card);
        }

        [HttpPost]
        [ActionName("Withdraw")]
        public async Task<IActionResult> AtmWithdraw(int id, AtmDataModel reqModel)
        {
            var card = await _context.AtmDatas.FirstOrDefaultAsync(x => x.Id == id);

            if(reqModel.Balance > card.Balance)
            {
                MessageModel model1 = new MessageModel(false, "Insufficient balance amount");
                return Json(model1);
            }

            card.Balance -= reqModel.Balance;
            _context.AtmDatas.Entry(card).State = EntityState.Modified;
            int result = _context.SaveChanges();
            string message = result > 0 ? "Withdraw Successful." : "Withdraw Failed.";

            MessageModel model = new MessageModel(result > 0, message);
            return Json(model);
        }

        [ActionName("ChangePin")]
        public async Task<IActionResult> AtmChangePin(int id)
        {
            var card = await _context.AtmDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return View("AtmChangePin", card);
        }
    }
}
