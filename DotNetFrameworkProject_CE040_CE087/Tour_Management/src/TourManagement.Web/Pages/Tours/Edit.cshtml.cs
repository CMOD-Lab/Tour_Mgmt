using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for editing an existing tour.
/// </summary>
public class EditModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the input view model.</summary>
    [BindProperty]
    public TourEditViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EditModel"/> class.
    /// </summary>
    public EditModel(ITourService tourService, IWebHostEnvironment environment, ILogger<EditModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the edit tour page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var tour = await _tourService.GetByIdAsync(id, cancellationToken);
            if (tour == null)
                return NotFound();

            // Map Entity to ViewModel manually
            Input = new TourEditViewModel
            {
                Id = tour.Id,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                ExistingPic = tour.Pic,
                IsActive = tour.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit page for tour id {TourId}", id);
            return RedirectToPage("Index");
        }
    }

    /// <summary>Handles POST requests to update a tour.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var existingTour = await _tourService.GetByIdAsync(Input.Id, cancellationToken);
            if (existingTour == null)
                return NotFound();

            string? picFileName = Input.ExistingPic;

            // Handle file upload
            if (Input.PicFile != null && Input.PicFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "tours");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Input.PicFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await Input.PicFile.CopyToAsync(fileStream, cancellationToken);
                picFileName = uniqueFileName;
            }

            // Update entity properties
            existingTour.TourName = Input.TourName;
            existingTour.Place = Input.Place;
            existingTour.Days = Input.Days;
            existingTour.Price = Input.Price;
            existingTour.Locations = Input.Locations;
            existingTour.TourInfo = Input.TourInfo;
            existingTour.Pic = picFileName;
            existingTour.IsActive = Input.IsActive;
            existingTour.ModifiedBy = User.Identity?.Name ?? "system";

            await _tourService.UpdateAsync(existingTour, cancellationToken);
            TempData["SuccessMessage"] = $"Tour '{Input.TourName}' was updated successfully!";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with id {TourId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the tour. Please try again.");
            return Page();
        }
    }
}
