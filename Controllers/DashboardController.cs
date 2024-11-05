using ASPBookProject.Data;
using ASPBookProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ASPBookProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/EditMedecin
        public async Task<IActionResult> EditMedecin(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medecin = await _context.Medecins
                .Include(m => m.Ordonnances)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medecin == null)
            {
                return NotFound();
            }

            var viewModel = new EditMedecinViewModel
            {
                Medecin = medecin
                // Ici, ajoutez d’autres listes si nécessaire, par exemple pour les rôles
            };

            return View(viewModel);
        }

        // POST: Dashboard/EditMedecin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedecin(string id, EditMedecinViewModel viewModel)
        {
            if (id != viewModel.Medecin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var medecin = await _context.Medecins
                        .Include(m => m.Ordonnances)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (medecin == null)
                    {
                        return NotFound();
                    }

                    // Mise à jour des propriétés du Medecin
                    medecin.Login_m = viewModel.Medecin.Login_m;
                    medecin.Role = viewModel.Medecin.Role;
                    medecin.Date_naissance_m = viewModel.Medecin.Date_naissance_m;

                    // Mise à jour des ordonnances ou d'autres collections si nécessaire
                    // Exemple : medecin.Ordonnances.Clear(); si vous devez réinitialiser

                    _context.Entry(medecin).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedecinExiste(viewModel.Medecin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Recharge les listes dans le ViewModel en cas d’erreur
            return View(viewModel);
        }

        private bool MedecinExiste(string id)
        {
            return _context.Medecins.Any(e => e.Id == id);
        }
    }

    public class EditMedecinViewModel
    {
        public Medecin Medecin { get; set; }
        // Ajoutez ici d’autres propriétés si nécessaire, par exemple des listes pour les rôles ou permissions
    }
}
