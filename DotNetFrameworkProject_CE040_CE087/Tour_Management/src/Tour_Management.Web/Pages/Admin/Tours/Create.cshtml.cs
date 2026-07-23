using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Tours;

/// <summary>
/// Page model for creating a new tour (admin).
/// </summary>
public class CreateModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CreateModel> _logger;

    /// <summary>Gets or sets the tour creation input model.</summary>
    [BindProperty]
    public TourCreateViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateModel"/> class.
    /// </summary>
    public CreateModel(ITourService tourService, IWebHostEnvironment environment, ILogger<CreateModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tour creation page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");
        return Page();
    }

    /// <summary>Handles POST requests for the tour creation form.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picFileName = null;
            if (Input.PicFile != null && Input.PicFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Tour_pics");
                Directory.CreateDirectory(uploadsFolder);
                picFileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.PicFile.FileName);
                var filePath = Path.Combine(uploadsFolder, picFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Input.PicFile.CopyToAsync(stream, cancellationToken);
            }

            var createDto = new TourCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                Pic = picFileName
            };

            await _tourService.CreateAsync(createDto, cancellationToken);
            _logger.LogInformation("Admin created new tour: {TourName}", Input.TourName);
            TempData["Success"] = $"Tour '{Input.TourName}' created successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", Input.TourName);
            TempData["Error"] = "An error occurred while creating the tour.";
            return Page();
        }
    }
}
