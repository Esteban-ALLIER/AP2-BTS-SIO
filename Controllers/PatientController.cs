using ASPBookProject.Data;
using ASPBookProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// Modèle ViewModel
public class PatientEditViewModel
{
    public Patient? Patient { get; set; }
    public List<Antecedent>? Antecedents { get; set; }
    public List<Allergie>? Allergies { get; set; }
    public List<int> SelectedAntecedentIds { get; set; } = new List<int>();
    public List<int> SelectedAllergieIds { get; set; } = new List<int>();
}

namespace ASPBookProject.Controllers
{
    public class PatientController : Controller
    {
        // 
        private readonly ApplicationDbContext _context;

        // Controleur, injection de dependance
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PatientController
        public ActionResult Index()
        {
            List<Patient> patients = new List<Patient>();
            patients = _context.Patients.ToList();
            return View(patients);
        }

        // Edit: PatientController 
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Antecedents)
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            var viewModel = new PatientEditViewModel
            {
                Patient = patient,
                Antecedents = await _context.Antecedents.ToListAsync(),
                Allergies = await _context.Allergies.ToListAsync(),
                SelectedAntecedentIds = patient.Antecedents.Select(a => a.AntecedentId).ToList() ?? new List<int>(),
                SelectedAllergieIds = patient.Allergies.Select(a => a.AllergieId).ToList() ?? new List<int>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientEditViewModel viewModel)
        {
            if (id != viewModel.Patient.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _context.Patients
                        .Include(p => p.Antecedents)
                        .Include(p => p.Allergies)
                        .FirstOrDefaultAsync(p => p.PatientId == id);

                    if (patient == null)
                    {
                        return NotFound();
                    }

                    // Mise à jour des propriétés du patient
                    patient.Nom_p = viewModel.Patient.Nom_p;
                    patient.Prenom_p = viewModel.Patient.Prenom_p;
                    patient.Sexe_p = viewModel.Patient.Sexe_p;
                    patient.Num_secu = viewModel.Patient.Num_secu;

                    // Mise à jour des allergies
                    patient.Allergies.Clear();
                    if (viewModel.SelectedAllergieIds != null)
                    {
                        var selectedAllergies = await _context.Allergies
                            .Where(a => viewModel.SelectedAllergieIds.Contains(a.AllergieId))
                            .ToListAsync();
                        foreach (var allergie in selectedAllergies)
                        {
                            patient.Allergies.Add(allergie);
                        }
                    }

                    // Mise à jour des antécédents
                    patient.Antecedents.Clear();
                    if (viewModel.SelectedAntecedentIds != null)
                    {
                        var selectedAntecedents = await _context.Antecedents
                            .Where(a => viewModel.SelectedAntecedentIds.Contains(a.AntecedentId))
                            .ToListAsync();
                        foreach (var antecedent in selectedAntecedents)
                        {
                            patient.Antecedents.Add(antecedent);
                        }
                    }
                    _context.Entry(patient).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(viewModel.Patient.PatientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si nous arrivons ici, quelque chose a échoué, réafficher le formulaire
            viewModel.Antecedents = await _context.Antecedents.ToListAsync();
            viewModel.Allergies = await _context.Allergies.ToListAsync();
            return View(viewModel);
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient); // Tu dois passer le patient à la vue Delete
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int patientId)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.PatientId == patientId);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return RedirectToAction("Index"); // Après suppression, redirige vers la liste des patients
        }
        public async Task<IActionResult> Details(int id)
        {
            // Récupère le patient en fonction de son ID, ainsi que ses antécédents et allergies
            var patient = await _context.Patients
                .Include(p => p.Antecedents)
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            // Si le patient n'est pas trouvé, retourne une erreur 404
            if (patient == null)
            {
                return NotFound();
            }

            // Création du modèle de vue avec les informations du patient et ses relations
            var viewModel = new PatientEditViewModel
            {
                Patient = patient,
                Antecedents = await _context.Antecedents.ToListAsync(),
                Allergies = await _context.Allergies.ToListAsync(),
                SelectedAntecedentIds = patient.Antecedents.Select(a => a.AntecedentId).ToList(),
                SelectedAllergieIds = patient.Allergies.Select(a => a.AllergieId).ToList()
            };

            // Retourne la vue de détails (lecture seule)
            return View(viewModel);
        }
    }
}
