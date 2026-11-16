using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechDesk.Data;
using TechDesk.Models;

namespace TechDesk.Controllers;

public class DevicesController : Controller
{
    private readonly AppDbContext _context;
    public DevicesController(AppDbContext context) { _context = context; }

    public async Task<IActionResult> Index(string? searchName, bool availableOnly = false)
    {
        var devices = _context.Devices.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchName))
            devices = devices.Where(d => d.Name.Contains(searchName));
        if (availableOnly)
            devices = devices.Where(d => !d.IsOnLoan);
        var list = await devices.OrderBy(d => d.AssetTag).ToListAsync();
        ViewBag.DeviceCount = list.Count;
        ViewBag.SearchName = searchName;
        ViewBag.AvailableOnly = availableOnly;
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        return device == null ? NotFound() : View(device);
    }

    [HttpGet]
    public IActionResult Create() => View(new Device { PurchaseYear = DateTime.Now.Year });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Device device)
    {
        if (await _context.Devices.AnyAsync(d => d.AssetTag == device.AssetTag))
            ModelState.AddModelError(nameof(device.AssetTag), "Another device already has that asset tag.");
        if (!ModelState.IsValid) return View(device);
        _context.Add(device);
        await _context.SaveChangesAsync();
        TempData["Message"] = $"{device.AssetTag} {device.Name} was added. The database has gained another resident.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var device = await _context.Devices.FindAsync(id);
        return device == null ? NotFound() : View(device);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Device device)
    {
        if (id != device.Id) return NotFound();
        if (await _context.Devices.AnyAsync(d => d.AssetTag == device.AssetTag && d.Id != id))
            ModelState.AddModelError(nameof(device.AssetTag), "Another device already has that asset tag.");
        if (!ModelState.IsValid) return View(device);
        try
        {
            // Load the tracked row and copy only editable fields from the form.
            // This avoids marking every property as modified on a detached object.
            var existingDevice = await _context.Devices.FindAsync(id);
            if (existingDevice == null) return NotFound();

            existingDevice.AssetTag = device.AssetTag;
            existingDevice.Name = device.Name;
            existingDevice.Category = device.Category;
            existingDevice.PurchaseYear = device.PurchaseYear;
            existingDevice.IsOnLoan = device.IsOnLoan;

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Devices.AnyAsync(d => d.Id == id)) return NotFound();
            throw;
        }
        TempData["Message"] = $"{device.AssetTag} was updated. Changes successfully survived the save button.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        return device == null ? NotFound() : View(device);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null)
            TempData["Message"] = "That device was already deleted. Someone got there first.";
        else
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"{device.AssetTag} was deleted. It has left the building.";
        }
        return RedirectToAction(nameof(Index));
    }
    [Route("/Devices/Error")]
    public IActionResult Error()
    {
        return View("Error");
    }
}
