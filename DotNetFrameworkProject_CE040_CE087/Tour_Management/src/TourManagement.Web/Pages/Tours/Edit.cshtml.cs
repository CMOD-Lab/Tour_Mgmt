using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces;
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
    /// Initializes a new instance of <see cref="EditModel"/>.
    /// </summary>
    public EditModel(ITourService tourService, IWebHostEnvironment environment, ILogger<EditModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the edit tour page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            TempData["ErrorMessage"] = "You must be an admin to edit tours.";
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var tour = await _tourService.GetByIdAsync(id);
            if (tour == null)
            {
                TempData["ErrorMessage"] = "Tour not found.";
                return RedirectToPage("Index");
            }

            // Map DTO to ViewModel manually
            Input = new TourEditViewModel
            {
                Id = tour.Id,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                CurrentPic = tour.Pic,
                IsActive = tour.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for edit, id {TourId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the tour.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to update an existing tour.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picFileName = Input.CurrentPic;

            // Handle file upload
            if (Input.PicFile != null && Input.PicFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "tours");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.PicFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await Input.PicFile.CopyToAsync(fileStream);
                picFileName = uniqueFileName;
            }

            // Map ViewModel to DTO manually
            var dto = new TourUpdateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                Pic = picFileName,
                IsActive = Input.IsActive
            };

            await _tourService.UpdateAsync(Input.Id, dto);
            TempData["SuccessMessage"] = $"Tour '{Input.TourName}' was updated successfully.";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "Tour not found.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with id {TourId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the tour. Please try again.");
            return Page();
        }
    }
}
