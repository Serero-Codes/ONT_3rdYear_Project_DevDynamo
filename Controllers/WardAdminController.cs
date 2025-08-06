using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;

namespace ONT_3rdyear_Project.Controllers
{
    public class WardAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Search for patients
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Patient>());
            }

            var patients = await _context.Patients
                .Where(p => p.FirstName.Contains(query) ||
                           p.LastName.Contains(query))
                           //p.IDNumber.Contains(query))
                .Include(p => p.Admissions)
                .ThenInclude(a => a.Bed)
                .ToListAsync();

            return View(patients);
        }
    }
}
