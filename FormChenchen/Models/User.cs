using System;
using System.Collections.Generic;

namespace FormChenchen.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Updatepass { get; set; }

    public string Lastname { get; set; } = null!;

    public string Names { get; set; } = null!;

    public bool Status { get; set; }
}
