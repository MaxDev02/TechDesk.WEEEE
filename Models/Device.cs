using System.ComponentModel.DataAnnotations;

namespace TechDesk.Models;

public class Device
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter an asset tag.")]
    [StringLength(10, ErrorMessage = "The asset tag cannot be longer than 10 characters.")]
    [Display(Name = "Asset tag")]
    public string AssetTag { get; set; } = "";

    [Required(ErrorMessage = "Please enter the device name.")]
    [StringLength(50, ErrorMessage = "The name cannot be longer than 50 characters.")]
    [Display(Name = "Device name")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Please enter a category.")]
    [StringLength(20, ErrorMessage = "The category cannot be longer than 20 characters.")]
    public string Category { get; set; } = "";

    [Range(2015, 2100, ErrorMessage = "The year bought must be from 2015 to 2100.")]
    [Display(Name = "Year bought")]
    public int PurchaseYear { get; set; }

    [Display(Name = "On loan?")]
    public bool IsOnLoan { get; set; }
}
