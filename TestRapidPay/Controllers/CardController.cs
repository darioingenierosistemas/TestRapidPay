using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestRapidPay.Models;

namespace TestRapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {

        private readonly RapidPayContext _context;


        public CardController(RapidPayContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("createCard")]
        public async Task<IActionResult> CreateCard(Card newCard)
        {
            // Validate card number format (you can implement your validation logic)
            if (string.IsNullOrEmpty(newCard.CardNumber) || newCard.CardNumber.Length != 15)
            {
                return BadRequest("Invalid card number format");
            }

            var card = new Card
            {
                CardNumber = newCard.CardNumber,
                Balance = newCard.Balance
            };

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.CardId }, card);
        }

       

        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> GetCardBalance(int cardId)
        {
            var card = await _context.Cards.FindAsync(cardId);

            if (card == null)
            {
                return NotFound("Card not found");
            }

            return Ok(card.Balance);
        }

        [Authorize]
        [HttpGet("card")]
        public async Task<IActionResult> GetCard(int cardId)
        {
            var card = await _context.Cards.FindAsync(cardId);

            if (card == null)
            {
                return NotFound("Card not found");
            }

            return Ok(card);
        }

    }
}
