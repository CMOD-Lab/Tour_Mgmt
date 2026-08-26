using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin edit tour page.
/// </summary>
public class EditTourModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditTourModel> _logger;

    /// <summary>Gets or sets the tour input model.</summary>
    [BindProperty]
    public TourCreateEditViewModel Input { get; set; } = new();

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="EditTourModel"/> class.</summary>
    public EditTourModel(ITourService tourService, IWebHostEnvironment environment, ILogger<EditTourModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the edit tour page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var tour = await _tourService.GetByIdAsync(id, cancellationToken);
            if (tour == null)
            {
                return NotFound();
            }

            // Manually map domain entity to ViewModel
            Input = new TourCreateEditViewModel
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                ExistingPic = tour.Pic
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for edit, ID: {TourId}", id);
            return RedirectToPage("/Admin/Tours");
        }
    }

    /// <summary>Handles POST requests for the edit tour form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            string? picFileName = Input.ExistingPic;

            // Handle file upload if new file provided
            if (Input.PicFile != null && Input.PicFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "tours");
                Directory.CreateDirectory(uploadsFolder);
                picFileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.PicFile.FileName);
                var filePath = Path.Combine(uploadsFolder, picFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Input.PicFile.CopyToAsync(stream, cancellationToken);
            }

            // Manually map ViewModel to domain entity
            var tour = new Tour
            {
                TourId = Input.TourId,
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                Pic = picFileName
            };

            await _tourService.UpdateAsync(tour, cancellationToken);
            _logger.LogInformation("Tour updated: {TourId}", Input.TourId);
            return RedirectToPage("/Admin/Tours");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour: {TourId}", Input.TourId);
            ErrorMessage = "An error occurred while updating the tour. Please try again.";
            return Page();
        }
    }
}
