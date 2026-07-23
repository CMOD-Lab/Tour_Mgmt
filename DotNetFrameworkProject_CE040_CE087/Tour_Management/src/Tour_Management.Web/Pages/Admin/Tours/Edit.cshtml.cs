using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Tours;

/// <summary>
/// Page model for editing an existing tour (admin).
/// </summary>
public class EditModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the tour edit input model.</summary>
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

    /// <summary>Handles GET requests for the tour edit page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        var dto = await _tourService.GetByIdAsync(id, cancellationToken);
        if (dto == null)
            return NotFound();

        Input = new TourEditViewModel
        {
            TourId = dto.TourId,
            TourName = dto.TourName,
            Place = dto.Place,
            Days = dto.Days,
            Price = dto.Price,
            Locations = dto.Locations,
            TourInfo = dto.TourInfo,
            Pic = dto.Pic,
            IsActive = dto.IsActive
        };
        return Page();
    }

    /// <summary>Handles POST requests for the tour edit form.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picFileName = Input.Pic;
            if (Input.PicFile != null && Input.PicFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Tour_pics");
                Directory.CreateDirectory(uploadsFolder);
                picFileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.PicFile.FileName);
                var filePath = Path.Combine(uploadsFolder, picFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Input.PicFile.CopyToAsync(stream, cancellationToken);
            }

            var updateDto = new TourUpdateDto
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

            await _tourService.UpdateAsync(Input.TourId, updateDto, cancellationToken);
            _logger.LogInformation("Admin updated tour id {TourId}", Input.TourId);
            TempData["Success"] = $"Tour '{Input.TourName}' updated successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour id {TourId}", Input.TourId);
            TempData["Error"] = "An error occurred while updating the tour.";
            return Page();
        }
    }
}
