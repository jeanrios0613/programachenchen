using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class VwUserRolesInfo
{  
    public string? id { get; set; }
    public string Username { get; set; } = null!;

    public bool? Status { get; set; }

    public string? Email { get; set; }

    public string Nombre { get; set; } = null!;

    public string? RolName { get; set; }

    public string? Description { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? DateUpdate { get; set; }

    public bool IndUpdate { get; set; }

    public string? UserCompania { get; set; }

    public int Gestion { get; set; }
}
