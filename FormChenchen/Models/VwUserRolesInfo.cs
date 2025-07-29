using System;
using System.Collections.Generic;

namespace FormChenchen.Models;

public partial class VwUserRolesInfo
{
    public string Username { get; set; } = null!;

    public bool Status { get; set; }

    public string? Email { get; set; }

    public string Nombre { get; set; } = null!;

    public string? RolName { get; set; }

    public string? Description { get; set; }

    public string? PasswordHash { get; set; }
}
