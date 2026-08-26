using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin create tour page.
/// </summary>
public class CreateTourModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CreateTourModel> _logger;

    /// <summary>Gets or sets the tour input model.</summary>
    [BindProperty]
    public TourCreateEditViewModel Input { get; set; } = new();

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="CreateTourModel"/> class.</summary>
    public CreateTourModel(ITourService tourService, IWebHostEnvironment environment, ILogger<CreateTourModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the create tour page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }
        return Page();
    }

    /// <summary>Handles POST requests for the create tour form submission.</summary>
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
            string? picFileName = null;

            // Handle file upload
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
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                Pic = picFileName
            };

            await _tourService.CreateAsync(tour, cancellationToken);
            _logger.LogInformation("Tour created: {TourName}", Input.TourName);
            return RedirectToPage("/Admin/Tours");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", Input.TourName);
            ErrorMessage = "An error occurred while creating the tour. Please try again.";
            return Page();
        }
    }
}
