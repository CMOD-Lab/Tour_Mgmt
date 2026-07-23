using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for creating a new tour.
/// </summary>
public class CreateModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ITourService tourService, IWebHostEnvironment environment, ILogger<CreateModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    [BindProperty]
    public TourCreateViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            string? picturePath = null;

            // Handle file upload
            if (Input.PictureFile != null && Input.PictureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "tour-pics");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.PictureFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Input.PictureFile.CopyToAsync(stream);
                picturePath = uniqueFileName;
            }

            var tour = new Tour
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                PicturePath = picturePath
            };

            await _tourService.CreateTourAsync(tour);
            TempData["SuccessMessage"] = $"Tour '{tour.TourName}' added successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            ErrorMessage = "An error occurred while adding the tour. Please try again.";
            return Page();
        }
    }
}
