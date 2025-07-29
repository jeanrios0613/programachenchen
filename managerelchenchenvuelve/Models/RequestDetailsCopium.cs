using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class RequestDetailsCopium
{
    public Guid Id { get; set; }

    public decimal QuantityToInvert { get; set; }

    public string ReasonForMoney { get; set; } = null!;

    public Guid RequestId { get; set; }

    public DateTime CreationDate { get; set; }
}
