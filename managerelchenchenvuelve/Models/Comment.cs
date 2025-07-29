using System;
using System.Collections.Generic;

namespace managerelchenchenvuelve.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public string ProcessInstanceId { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public string StageName { get; set; } = null!; 
}
