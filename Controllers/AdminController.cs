using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.ViewModels;

namespace ONT_3rdyear_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> ListMedication()
        {
            var medications = await _context.Medications.ToListAsync();
            return View(medications);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications.FirstOrDefaultAsync(m=>m.MedicationId == id);   
            return View(medication);

        }

        public IActionResult CreateMedication()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMedication(Medication medication)
        {
            try
            {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListMedication));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error has occured");
            }
            return View(medication);
        }

        public async Task<IActionResult> EditMedication(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var medication = await _context.Medications.FindAsync(id);
            if(medication == null)
            {
                return NotFound();
            }
            return View(medication);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedication(int id, Medication medication)
        {
            if(id != medication.MedicationId)
            {
                return NotFound();
            }
            try
            {
                _context.Update(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListMedication));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Medications.Any(m => m.MedicationId == id))
                    return NotFound();
                else
                    throw;
            }
            return View(medication);
        }

        //TODO: soft delete for medication





        //crud for managing employees
        public async Task<IActionResult> Employees()
        {
            var users = await _userManager.Users.ToListAsync();

            var employees = new List<EmployeeViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                employees.Add(new EmployeeViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "N/A"
                });
            }

            return View(employees);
        }
    }
}
