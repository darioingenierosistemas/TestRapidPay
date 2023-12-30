using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestRapidPay.Models;
using TestRapidPay.Services;

namespace TestRapidPay.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly RapidPayContext _context;

        public PaymentController(RapidPayContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Pay(int cardId, decimal amount)
        {
            var card = await _context.Cards.FindAsync(cardId);

            if (card == null)
            {
                return NotFound("Card not found");
            }

            if (card.Balance < amount)
            {
                return BadRequest("Insufficient balance");
            }

            card.Balance -= amount;
            decimal currentFee = UniversalFeesExchange.Instance.GetCurrentFee();
            decimal feeAmount = amount * currentFee;

            card.Balance -= amount + feeAmount;

            var payments = new Payment
            {
                CardId = cardId,
                Amount = amount,
                Fee = feeAmount,
                PaymentDateTime = DateTime.UtcNow
            };

            _context.Payments.Add(payments);

            await _context.SaveChangesAsync();


            return Ok("Payment Done");
        }
    }
}
