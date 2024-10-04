using System;
using Microsoft.AspNetCore.Identity;

namespace ASPBookProject.Models;

public class Medecin : IdentityUser
{
    public DateTime Date_naissance_m { get; set; }
    public required string Login_m { get; set; }
    public required string Role { get; set; }
    public List<Ordonnance> Ordonnances { get; set; } = new();
}
