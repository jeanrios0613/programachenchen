using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class Enterprise
{
    public Guid Id { get; set; }

    public string? BusinessName { get; set; }

    public string? BusinessDescription { get; set; }

    public string EconomicActivity { get; set; } = null!;

    public string Instagram { get; set; } = null!;

    public string? Ruc { get; set; }

    public string WebSite { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public Guid RequestId { get; set; }

    public string BusinessTime { get; set; } = null!;

    public string Corregimiento { get; set; } = null!;

    public string District { get; set; } = null!;

    public decimal MonthlySales { get; set; }

    public DateTime OperationsStartDate { get; set; }

    public string Province { get; set; } = null!;

    public decimal ProyectedSales { get; set; }

    public virtual Request Request { get; set; } = null!;
}
