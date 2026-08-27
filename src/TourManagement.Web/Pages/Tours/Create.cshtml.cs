using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for creating a new tour.
/// </summary>
[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the tour creation view model.</summary>
    [BindProperty]
    public TourCreateViewModel Tour { get; set; } = new();

    /// <summary>Initializes a new instance of CreateModel.</summary>
    public CreateModel(ITourService tourService, IWebHostEnvironment environment, ILogger<CreateModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the create tour page.</summary>
    public IActionResult OnGet()
    {
        return Page();
    }

    /// <summary>Handles POST requests to create a new tour.</summary>
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

            // Manual mapping from ViewModel to DTO
            var createDto = new TourCreateDto
            {
                TourName = Tour.TourName,
                Place = Tour.Place,
                Days = Tour.Days,
                Price = Tour.Price,
                Locations = Tour.Locations,
                TourInfo = Tour.TourInfo,
                Pic = picFileName
            };

            await _tourService.CreateAsync(createDto, cancellationToken);
            TempData["SuccessMessage"] = "Tour added successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", Tour.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the tour.");
            return Page();
        }
    }
}
