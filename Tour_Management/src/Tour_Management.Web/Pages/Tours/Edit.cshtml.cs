using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for editing an existing tour.
/// </summary>
public class EditModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITourService tourService, IWebHostEnvironment environment, ILogger<EditModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    [BindProperty]
    public TourEditViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        try
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
                return NotFound();

            Input = new TourEditViewModel
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                ExistingPicturePath = tour.PicturePath
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for edit, ID {TourId}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picturePath = Input.ExistingPicturePath;

            if (Input.PictureFile != null && Input.PictureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "tour-pics");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.PictureFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Input.PictureFile.CopyToAsync(stream);
                picturePath = uniqueFileName;
            }

            var tour = new Tour
            {
                TourId = Input.TourId,
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                PicturePath = picturePath
            };

            await _tourService.UpdateTourAsync(tour);
            TempData["SuccessMessage"] = $"Tour '{tour.TourName}' updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID {TourId}", Input.TourId);
            ErrorMessage = "An error occurred while updating the tour. Please try again.";
            return Page();
        }
    }
}
