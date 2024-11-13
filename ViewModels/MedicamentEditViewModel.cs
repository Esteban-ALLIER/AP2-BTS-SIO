using System;
using System.ComponentModel.DataAnnotations;
using ASPBookProject.Models;

namespace ASPBookProject.ViewModels;

public class MedicamentEditViewModel
{
    [Required]
    public Medicament? Medicament { get; set; }
    public List<Antecedent>? Antecedents { get; set; }
    public List<Allergie>? Allergies { get; set; }
    public List<int> SelectedAntecedentIds { get; set; } = new List<int>();
    public List<int> SelectedAllergieIds { get; set; } = new List<int>();
}
