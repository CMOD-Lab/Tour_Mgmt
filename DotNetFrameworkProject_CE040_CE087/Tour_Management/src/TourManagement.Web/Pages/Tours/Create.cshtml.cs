using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for creating a new tour.
/// </summary>
public class CreateModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the tour create view model.</summary>
    [BindProperty]
    public TourCreateViewModel Tour { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CreateModel"/>.
    /// </summary>
    public CreateModel(ITourService tourService, IWebHostEnvironment environment, ILogger<CreateModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the create tour page.
    /// </summary>
    public void OnGet()
    {
    }

    /// <summary>
    /// Handles POST requests to create a new tour.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picFileName = null;

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

            var dto = new TourCreateDto
            {
                TourName = Tour.TourName,
                Place = Tour.Place,
                Days = Tour.Days,
                Price = Tour.Price,
                Locations = Tour.Locations,
                TourInfo = Tour.TourInfo,
                Pic = picFileName,
                CreatedBy = HttpContext.Session.GetString("UserEmail") ?? "system"
            };

            await _tourService.CreateAsync(dto, cancellationToken);
            TempData["SuccessMessage"] = "Tour added successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", Tour.TourName);
            TempData["ErrorMessage"] = "An error occurred while adding the tour.";
            return Page();
        }
    }
}
