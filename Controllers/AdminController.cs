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
        public async Task<IActionResult> EditMedication(Medication medication)
        {
            if (ModelState.IsValid)
            {
                _context.Medications.Update(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction("Medications");
            }
            return View(medication);
        }

        //TODO: soft delete for medication
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication != null)
            {
                medication.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Medications");
        }

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

        public async Task<IActionResult> EditEmployee(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Employees");
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Employees");
        }

        // WARD MANAGEMENT
        public async Task<IActionResult> Wards()
        {
            var wards = await _context.Wards.Where(w => w.IsActive).ToListAsync();
            return View(wards);
        }

        public IActionResult AddWard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWard(Ward ward)
        {
            if (ModelState.IsValid)
            {
                _context.Wards.Add(ward);
                await _context.SaveChangesAsync();
                return RedirectToAction("Wards");
            }
            return View(ward);
        }

        public async Task<IActionResult> EditWard(int id)
        {
            var ward = await _context.Wards.FindAsync(id);
            return View(ward);
        }

        [HttpPost]
        public async Task<IActionResult> EditWard(Ward ward)
        {
            if (ModelState.IsValid)
            {
                _context.Wards.Update(ward);
                await _context.SaveChangesAsync();
                return RedirectToAction("Wards");
            }
            return View(ward);
        }

        public async Task<IActionResult> DeleteWard(int id)
        {
            var ward = await _context.Wards.FindAsync(id);
            if (ward != null)
            {
                ward.IsActive = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Wards");
        }

        //BEDS MANAGEMENT
        public async Task<IActionResult> Beds()
        {
            var beds = await _context.Beds.Include(b => b.Ward).Where(b => !b.IsDeleted).ToListAsync();
            return View(beds);
        }

        public IActionResult AddBed()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBed(Bed bed)
        {
            if (ModelState.IsValid)
            {
                _context.Beds.Add(bed);
                await _context.SaveChangesAsync();
                return RedirectToAction("Beds");
            }
            return View(bed);
        }

        public async Task<IActionResult> EditBed(int id)
        {
            var bed = await _context.Beds.FindAsync(id);
            return View(bed);
        }

        [HttpPost]
        public async Task<IActionResult> EditBed(Bed bed)
        {
            if (ModelState.IsValid)
            {
                _context.Beds.Update(bed);
                await _context.SaveChangesAsync();
                return RedirectToAction("Beds");
            }
            return View(bed);
        }

        public async Task<IActionResult> DeleteBed(int id)
        {
            var bed = await _context.Beds.FindAsync(id);
            if (bed != null)
            {
                bed.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Beds");
        }

        // CONSUMABLES MANAGEMENT
        //public async Task<IActionResult> Consumables()
        //{
        //    var consumables = await _context.Consumables.Where(c => !c.IsDeleted).ToListAsync();
        //    return View(consumables);
        //}

        //public IActionResult AddConsumable() => View();

        //[HttpPost]
        //public async Task<IActionResult> AddConsumable(Consumable item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Consumables.Add(item);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Consumables");
        //    }
        //    return View(item);
        //}

        //public async Task<IActionResult> EditConsumable(int id)
        //{
        //    var item = await _context.Consumables.FindAsync(id);
        //    return View(item);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditConsumable(Consumable item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Consumables.Update(item);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Consumables");
        //    }
        //    return View(item);
        //}

        //public async Task<IActionResult> DeleteConsumable(int id)
        //{
        //    var item = await _context.Consumables.FindAsync(id);
        //    if (item != null)
        //    {
        //        item.IsDeleted = true;
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction("Consumables");
        //}

        //ALLERGY MANAGEMENT
        public async Task<IActionResult> Allergies()
        {
            var list = await _context.Allergies.Where(a => !a.IsDeleted).ToListAsync();
            return View(list);
        }

        public IActionResult AddAllergy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAllergy(Allergy model)
        {
            if (ModelState.IsValid)
            {
                _context.Allergies.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Allergies");
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteAllergy(int id)
        {
            var allergy = await _context.Allergies.FindAsync(id);
            if (allergy != null)
            {
                allergy.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Allergies");
        }

        // HOSPITAL INFO MANAGEMENT
        public async Task<IActionResult> HospitalInfo()
        {
            var info = await _context.HospitalInfo.FirstOrDefaultAsync();
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> HospitalInfo(HospitalInfo info)
        {
            _context.HospitalInfo.Update(info);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Hospital Info Updated";
            return RedirectToAction("HospitalInfo");
        }
    }
}
