using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.StaticHelper;
using ONT_3rdyear_Project.ViewModels;

namespace ONT_3rdyear_Project.Controllers
{
    public class ConsumableController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ConsumableController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //Dashboard for Consumables Management
        public IActionResult Dashboard()
        {
           
            return View();
        }
        public async Task<IActionResult> Wards()
        {
            var wards = await _context.Wards.ToListAsync();
            return View(wards);
        }
        public async Task<IActionResult> Index()
        {
            var consumables = await _context.Consumables.ToListAsync();
            return View(consumables);
        }
        //Add New Consumable controller
        [HttpGet]
        [Route("Consumable/NewConsumable")]
        public async Task<IActionResult> NewConsumable(string? name, bool partial = false)
        {
            var consumable = await _context.Consumables.FindAsync(name);           
            ViewBag.CategoryList = ConsumableCategoryHelper.GetCategories();              

            return PartialView("NewConsumable");         

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewConsumable(string name, Consumable model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, See your system administrator.");
                return View(model);
            }
            bool nameExists = await _context.Consumables
          .AnyAsync(c => c.Name.ToLower() == model.Name.ToLower());

            if (nameExists)
            {
                ModelState.AddModelError("isAvailable", "Consumable already exists.");
            }         

           
            _context.Consumables.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Consumable");                 
               
           
        }
        //Edit Consumable controller
        [HttpGet]
        [Route("Consumable/EditConsumable/{id}")]
        public async Task<IActionResult> EditConsumable(int id)
        {
            var consumable = await _context.Consumables.FirstOrDefaultAsync(c => c.ConsumableId == id);
            if (consumable == null)
            {
                return NotFound();
            }
            ViewBag.CategoryList = ConsumableCategoryHelper.GetCategories();
            return View(consumable);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConsumable(int id , Consumable model)
        {
            if (id != model.ConsumableId)
                return NotFound();

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Consumables.Any(p => p.ConsumableId == id))
                    return NotFound();
                else
                    throw;
            }
            ViewBag.CategoryList = ConsumableCategoryHelper.GetCategories();
            
            return RedirectToAction("Index", "Consumable");
        }
        //Soft Delete Consumable controller
        public async Task<IActionResult> DeleteConsumable(int id)
        {
            var item = await _context.Consumables.FindAsync(id);
            if (item != null)
            {
                item.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Consumables");
        }
        //View Consumables inside a Ward
        public async Task<IActionResult> WardConsumables()
        {
            var data = await _context.WardConsumables.Where(wc => wc.Consumable.IsDeleted == false)
                  .OrderBy(wc => wc.Ward.Name)
                  .Include(wc => wc.Ward)
                  .Include(wc => wc.Consumable)
                  .Select(wc => new WardConsumableViewModel
                  {
                      WardName = wc.Ward.Name,
                      WardID = wc.WardID,
                      ConsumableID = wc != null ? wc.ConsumableID : 0,
                      ConsumableName = wc != null ? wc.Consumable.Name : "No Consumable",
                      Category = wc != null ? wc.Consumable.Category : "-",
                      SystemQuantity = wc != null ? wc.Consumable.StockTakeItems
                      .Where(sti => sti.ConsumableID == wc.ConsumableID && sti.StockTake.WardID == wc.WardID)
                      .OrderByDescending(sti => sti.StockTake.StockTakeDate)
                      .Select(sti => sti.SystemQuantity)
                      .FirstOrDefault ():0,
                      LastUpdated = wc.Consumable.StockTakeItems
                      .Where(sti => sti.ConsumableID == wc.ConsumableID && sti.StockTake.WardID == wc.WardID)
                      .OrderByDescending(sti => sti.StockTake.StockTakeDate)
                      .Select(sti => sti.StockTake.StockTakeDate)
                      .FirstOrDefault()

                  }).ToListAsync();

            return View(data);
        }
        //Retrieve data for stock take as Json
        public async Task<IActionResult> GetConsumablesByWard(int wardId)
        {

            var consumables = await _context.WardConsumables
                .Where(wc => wc.WardID == wardId)
                 .Include(wc => wc.Ward)  // Include StockTakeItems if needed
                .Include(wc => wc.Consumable)
                 .ThenInclude(c => c.StockTakeItems)
                    .ThenInclude(sti => sti.StockTake)
                .Select(wc => new WardConsumableViewModel
                {
                    WardID = wardId,
                    WardName = wc.Ward.Name,
                    ConsumableID = wc.ConsumableID,
                    ConsumableName = wc.Consumable.Name,
                    Category = wc.Consumable.Category,                  
                    SystemQuantity = wc.Consumable.StockTakeItems
                    .Where(sti => sti.ConsumableID == wc.ConsumableID && sti.StockTake.WardID == wc.WardID)
                    .OrderByDescending(sti => sti.StockTake.StockTakeDate)
                    .Select(sti => sti.SystemQuantity)
                    .FirstOrDefault(),
                    LastUpdated = wc.Consumable.StockTakeItems
                    .Where(sti => sti.ConsumableID == wc.ConsumableID && sti.StockTake.WardID == wc.WardID)
                    .OrderByDescending(sti => sti.StockTake.StockTakeDate)
                    .Select(sti => sti.StockTake.StockTakeDate)
                    .FirstOrDefault()

                })
                .ToListAsync();

            return Json(consumables);

        }
        //Add Consumables to a ward
        [HttpGet]
        public IActionResult AddConsumableToWard(int wardId)
        {
            // Set ViewBag lists
            ViewBag.ConsumableList = new SelectList(_context.Consumables.ToList(), "ConsumableId", "Name");

           var ward = _context.Wards.FirstOrDefault(w => w.WardID == wardId);



            return PartialView("AddConsumableToWard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConsumableToWard(int WardId, int ConsumableID, int Quantity)
        {
            if (WardId == 0 || ConsumableID == 0)
            {
                ViewBag.WardList = new SelectList(_context.Wards.ToList(), "WardID", "Name");
                ViewBag.ConsumableList = new SelectList(_context.Consumables.ToList(), "ConsumableId", "Name");
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var consumable = await _context.WardConsumables
             .Include(wc => wc.Ward)
             .Include(wc => wc.Consumable)
             .FirstOrDefaultAsync(wc => wc.WardID == WardId && wc.ConsumableID == ConsumableID);
            if (consumable != null)
            {
                ModelState.AddModelError("", "Consumable already exists in this ward.");
                ViewBag.WardList = new SelectList(_context.Wards.ToList(), "WardID", "Name");
                ViewBag.ConsumableList = new SelectList(_context.Consumables.ToList(), "ConsumableId", "Name");
                return View();
            }
            else
            {
                var stocktake = new StockTake
                {
                    WardID = WardId,
                    StockTakeDate = DateTime.Now,
                    TakenBy = user.Id,
                    TakenByUser = user

                };
                _context.StockTakes.Add(stocktake);
                await _context.SaveChangesAsync();

                var stockTakeItem = new StockTakeItem
                {
                    StockTakeID = stocktake.StockTakeID,
                    ConsumableID = ConsumableID,
                    QuantityCounted = Quantity,
                    SystemQuantity = Quantity
                };
                var wardConsumable = new WardConsumable
                {
                    ConsumableID = ConsumableID,
                    WardID = WardId,

                };

                _context.StockTakeItems.Add(stockTakeItem);
                _context.WardConsumables.Add(wardConsumable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("WardConsumables", "Consumable");
        }
        //Add Consumable direct from the ward masterList
        public async Task<IActionResult> AddConsumableDirect()
        {
            ViewBag.WardList = new SelectList(_context.Wards.ToList(), "WardID", "Name");
            ViewBag.ConsumableList = new SelectList(_context.Consumables.ToList(), "ConsumableId", "Name");
            return PartialView("AddConsumableDirect");
        }
        [HttpPost]
        public async Task<IActionResult> AddConsumableDirect(WardConsumable model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.GetUserAsync(User);
                
                var existingConsumable = await _context.WardConsumables
                    .FirstOrDefaultAsync(wc => wc.WardID == model.WardID && wc.ConsumableID == model.ConsumableID);
                
                if (existingConsumable != null)
                {
                    ModelState.AddModelError("", "This consumable already exists in the selected ward.");
                }
                else
                {
                    var stocktake = new StockTake
                    {
                        WardID = model.WardID,
                        StockTakeDate = DateTime.Now,
                        TakenBy = user.Id,
                        TakenByUser = user
                    };
                    _context.StockTakes.Add(stocktake);
                    await _context.SaveChangesAsync();

                    var stockTakeItem = new StockTakeItem
                    {
                        StockTakeID = stocktake.StockTakeID,
                        ConsumableID = model.ConsumableID,
                        QuantityCounted = model.Quantity,
                        SystemQuantity = model.Quantity
                    };
                    _context.WardConsumables.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("WardConsumables");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
            }
            return View("WardConsumables");
        }
        //Creating a New Stock Take request for a Ward
        [HttpGet]
        public async Task<IActionResult> StockTake(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            ViewBag.Wards = new SelectList(_context.Wards, "WardID", "Name");

            var model = new StockTakeViewModel
            {
                TakenByID = user.Id,

                Items = new List<StockTakeItemEntry>() // empty list initially
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StockTake(StockTakeViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            ViewBag.Wards = new SelectList(_context.Wards, "WardID", "Name");
            var wardConsumables = await _context.WardConsumables
                .Where(wc => wc.WardID == model.WardID)
                .Include(wc => wc.Consumable)
                .ToListAsync();
            var stockTake = new StockTake
            {
                WardID = model.WardID,
                StockTakeDate = DateTime.Now,
                TakenBy = user.Id,
                TakenByUser = user,

                StockTakes = model.Items.Select(i => new StockTakeItem
                {
                    ConsumableID = i.ConsumableID,
                    QuantityCounted = i.CountedQuantity,
                    SystemQuantity = wardConsumables
                        .Where(wc => wc.ConsumableID == i.ConsumableID)
                        .Select(wc => wc.Quantity)
                        .FirstOrDefault(),
                }).ToList()
            };

            if (!ModelState.IsValid)
            {
                var Wards = await _context.Wards
                .Select(w => new SelectListItem { Value = w.WardID.ToString(), Text = w.Name })
                .ToListAsync();
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // or use logging
                }

                return View(model);
            }

            _context.Add(stockTake);
            //Update Available Quantity
            foreach (var item in model.Items)
            {
                var wardConsumable = await _context.WardConsumables.FirstOrDefaultAsync(wc => wc.WardID == model.WardID && wc.ConsumableID == item.ConsumableID);


                if (wardConsumable != null && item.CountedQuantity != 0)
                {
                    wardConsumable.Quantity = item.CountedQuantity;
                    _context.Update(wardConsumable);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
        //Create Stock request controller
        [HttpGet]
        public IActionResult OrderConsumables()
        {
            var viewModel = new Order
            {
                ConsumableOrders = new List<ConsumableOrder> { new()}
            }; 
            ViewBag.Wards = new SelectList(_context.Wards, "WardID", "Name");
            return View(viewModel);
        }
        //This controller allows users to request consumables for a specific ward using Ajax.
        [HttpGet]
        public IActionResult RequestConsumablesByWard(int WardId)
        {
            // Filter consumables by the wardID
            var WardConsumables = _context.Consumables
                .Where(c => c.WardConsumables.Any(ws => ws.WardID == WardId))
                .Select(c => new
                {
                    c.ConsumableId,
                    c.Name
                })
                .ToList();

            return Json(WardConsumables);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderConsumables(Order model, int i)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Consumables = new SelectList(_context.Consumables, "ConsumableId", "Name");
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            var s = await _context.Suppliers.FindAsync(1);
            if (user == null) return Unauthorized();
            model.OrderedBy = user.Id;
            //temp
            model.SupplierID = s.SupplierId;
            model.OrderDate = DateTime.Now;
            model.Status = "pending";          
            model.ConsumableOrders
            .Where(i => i.ConsumableId > 0 && i.QuantityRequested > 0)
            .Select(i =>  new ConsumableOrder 
            {
                
                ConsumableId = i.ConsumableId,
                QuantityRequested = i.QuantityRequested,
                Reason = i.Reason 
            }).ToList();
            model.OrderQuantity = model.ConsumableOrders.Sum(i => i.QuantityRequested);
            ViewBag.Wards = new SelectList(_context.Wards, "WardID", "Name");
            ViewBag.Consumables = new SelectList(_context.Consumables.Include(c => c.WardConsumables).Where(c => c.WardConsumables.Any(ws => ws.WardID == i)), "ConsumableId", "Name");
            _context.Orders.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
