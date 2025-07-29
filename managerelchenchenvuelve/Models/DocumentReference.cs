using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class DocumentReference
{
    public Guid Id { get; set; }

    public string ProcessInstanceId { get; set; } = null!;

    public string? DocumentTitle { get; set; }

    public string? StageName { get; set; }

    public bool IsDeleted { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
