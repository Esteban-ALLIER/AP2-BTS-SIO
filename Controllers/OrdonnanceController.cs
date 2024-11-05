using ASPBookProject.Data;
using ASPBookProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ASPBookProject.Controllers;

[Authorize]
public class OrdonnanceController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrdonnanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int patientId)
    {
        // Récupérer les ordonnances pour le patient avec son ID
        var ordonnances = await _context.Ordonnances
            .Include(o => o.Patient)      // Inclure les informations du patient
            .Include(o => o.Medicaments)  // Inclure les médicaments de l'ordonnance
            .Where(o => o.PatientId == patientId) // Filtrer par patientId
            .ToListAsync();

        ViewBag.PatientId = patientId;

        return View(ordonnances);
    }



    [HttpGet]
    public async Task<IActionResult> Create(int patientId)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null)
        {
            return NotFound();
        }

        var viewModel = new AddOrdonnanceViewModel
        {
            PatientId = patientId,
            NomPatient = $"{patient.Nom_p} {patient.Prenom_p}",
            Antecedents = await _context.Antecedents.ToListAsync(),
            Allergies = await _context.Allergies.ToListAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddOrdonnanceViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Antecedents = await _context.Antecedents.ToListAsync();
            viewModel.Allergies = await _context.Allergies.ToListAsync();
            return View(viewModel);
        }

        // Récupérer le patient
        var patient = await _context.Patients
            .Include(p => p.Antecedents)
            .Include(p => p.Allergies)
            .FirstOrDefaultAsync(p => p.PatientId == viewModel.PatientId);

        if (patient == null)
        {
            return NotFound();
        }

        // Récupérer le médecin connecté
        var medecin = await _context.Medecins
            .FirstOrDefaultAsync(m => m.Id == User.Identity.Name); // Id du médecin connecté

        if (medecin == null)
        {
            return Unauthorized(); // Si le médecin connecté n'existe pas
        }

        // Créer l'ordonnance avec les données fournies
        var ordonnance = new Ordonnance
        {
            Posologie = viewModel.Posologie,
            Duree_traitement = viewModel.Duree_traitement,
            Instructions_specifique = viewModel.Instructions_specifique,
            PatientId = viewModel.PatientId, // Assigner l'ID du patient
            Patient = patient,               // Assigner le patient
            MedecinId = medecin.Id,          // Assigner l'ID du médecin
            Medecin = medecin                // Assigner le médecin
        };

        _context.Ordonnances.Add(ordonnance);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", new { patientId = viewModel.PatientId });
    }
}