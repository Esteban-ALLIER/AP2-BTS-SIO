using ASPBookProject.Data;
using ASPBookProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPBookProject.Controllers
{
    public class StatistiqueController : Controller
    {

        private readonly UserManager<Medecin> _userManager;
        private readonly ApplicationDbContext _context;
        
        public StatistiqueController(UserManager<Medecin> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: StatistiqueController
        public async Task<ActionResult> Index()
        {
            List<Medicament> medicaments = new List<Medicament>();
            medicaments = await _context.Medicaments
                                .ToListAsync();
            medicaments.OrderByDescending(o => o.compteur);
            return View(medicaments);
        }

    }
}
