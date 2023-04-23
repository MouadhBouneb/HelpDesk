namespace HelpDesk4GL.Models;

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
public class User : IdentityUser
{
    // public int Id { get; set; }

    public String? fullName { get; set; }

    public String? role { get; set; }

    // public String? login { get; set; }

    // public String? password { get; set; }

    public ICollection<Reclamation>? Reclamations { get; set; }

    public DateTime? createdAt { get; set; }

    public DateTime? updatedAt { get; set; }

}