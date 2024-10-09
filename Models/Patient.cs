using System;
using System.ComponentModel.DataAnnotations;

namespace ASPBookProject.Models;
public enum Sexe { Masculin, Féminin}

public class Patient
{
    [Key]
    public int PatientId { get; set; }
    public required string Nom_p { get; set; }
    public required string Prenom_p { get; set; }
    public required Sexe Sexe_p { get; set; }

    [RegularExpression(@"^\d{7}$", ErrorMessage = "Le numéro de sécurité dois posseder 7 chiffres")]
    public required String Num_secu { get; set; }
    public List<Antecedent> Antecedents { get; set; } = new();
    public List<Allergie> Allergies { get; set; } = new();
    public List<Ordonnance>? Ordonnances { get; set; }
    
}
