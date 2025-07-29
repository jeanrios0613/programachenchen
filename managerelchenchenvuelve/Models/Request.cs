using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class Request
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string Suggestion { get; set; } = null!;

    public int Type { get; set; }

    public virtual Contact? Contact { get; set; }

    public virtual Enterprise? Enterprise { get; set; }

    public virtual RequestDetail? RequestDetail { get; set; }
}
