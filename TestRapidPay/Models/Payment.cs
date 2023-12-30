using System;
using System.Collections.Generic;

namespace TestRapidPay.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int CardId { get; set; }

    public decimal Amount { get; set; }

    public decimal Fee { get; set; }

    public DateTime? PaymentDateTime { get; set; }

    //public virtual Card Card { get; set; } = null!;
}
