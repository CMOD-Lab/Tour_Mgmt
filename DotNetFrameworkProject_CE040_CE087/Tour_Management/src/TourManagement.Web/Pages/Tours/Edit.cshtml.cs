using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
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

    /// <summary>Gets or sets the tour edit view model.</summary>
    [BindProperty]
    public TourEditViewModel Tour { get; set; } = new();

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
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _tourService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            Tour = new TourEditViewModel
            {
                Id = dto.Id,
                TourName = dto.TourName,
                Place = dto.Place,
                Days = dto.Days,
                Price = dto.Price,
                Locations = dto.Locations,
                TourInfo = dto.TourInfo,
                ExistingPic = dto.Pic
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
    /// Handles POST requests to update a tour.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picFileName = Tour.ExistingPic;

            // Handle file upload
            if (Tour.PicFile != null && Tour.PicFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Tour_pics");
                Directory.CreateDirectory(uploadsFolder);
                picFileName = $"{Guid.NewGuid()}_{Tour.PicFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, picFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Tour.PicFile.CopyToAsync(stream, cancellationToken);
            }

            var dto = new TourUpdateDto
            {
                TourName = Tour.TourName,
                Place = Tour.Place,
                Days = Tour.Days,
                Price = Tour.Price,
                Locations = Tour.Locations,
                TourInfo = Tour.TourInfo,
                Pic = picFileName,
                ModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "system"
            };

            await _tourService.UpdateAsync(Tour.Id, dto, cancellationToken);
            TempData["SuccessMessage"] = "Tour updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with id {TourId}", Tour.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the tour.";
            return Page();
        }
    }
}
