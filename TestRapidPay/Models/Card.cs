using System;
using System.Collections.Generic;

namespace TestRapidPay.Models;

public partial class Card
{
    public int CardId { get; set; }

    public string CardNumber { get; set; } = null!;

    public decimal? Balance { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
