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

    /// <summary>Gets or sets the input view model.</summary>
    [BindProperty]
    public TourCreateViewModel Input { get; set; } = new();

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
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            TempData["ErrorMessage"] = "You must be an admin to add tours.";
            return RedirectToPage("/Admin/Login");
        }
        return Page();
    }

    /// <summary>
    /// Handles POST requests to create a new tour.
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
            string? picFileName = null;

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

            // Map ViewModel to DTO manually (Web layer responsibility)
            var dto = new TourCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                Pic = picFileName
            };

            await _tourService.CreateAsync(dto);
            TempData["SuccessMessage"] = $"Tour '{Input.TourName}' was created successfully.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", Input.TourName);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the tour. Please try again.");
            return Page();
        }
    }
}
