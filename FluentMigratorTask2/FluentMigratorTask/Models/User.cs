using System;
using System.Collections.Generic;

namespace FluentMigratorTask.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int Age { get; set; }
}
