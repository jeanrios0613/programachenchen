using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? RolName { get; set; }

    public string? ActiveDirectoryGroup { get; set; }

    public string? Description { get; set; }

    public bool IsActiveDirectorySync { get; set; }

    public bool Status { get; set; }
}
