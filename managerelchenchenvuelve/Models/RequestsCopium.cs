using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class RequestsCopium
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string Suggestion { get; set; } = null!;
}
