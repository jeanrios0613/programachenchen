using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class Contact
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string IdentificationNumber { get; set; } = null!;

    public string IdentificationType { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public Guid RequestId { get; set; }

    public virtual Request Request { get; set; } = null!;
}
