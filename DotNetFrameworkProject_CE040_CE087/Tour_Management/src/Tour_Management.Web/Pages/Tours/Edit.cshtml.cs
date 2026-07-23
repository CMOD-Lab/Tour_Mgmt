using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>Page model for editing an existing tour.</summary>
public class EditModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITourService tourService, IWebHostEnvironment environment, ILogger<EditModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    [BindProperty]
    public TourEditViewModel Tour { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var dto = await _tourService.GetByIdAsync(id, cancellationToken);
            if (dto == null) return NotFound();

            Tour = TourEditViewModel.FromDto(dto);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for edit, ID {TourId}", id);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Users/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            string? picFileName = Tour.ExistingPic;

            // Handle new file upload
            if (Tour.PicFile != null && Tour.PicFile.Length > 0)
            {
                var tourPicsPath = Path.Combine(_environment.WebRootPath, "tour-pics");
                Directory.CreateDirectory(tourPicsPath);

                picFileName = $"{Guid.NewGuid()}_{Path.GetFileName(Tour.PicFile.FileName)}";
                var filePath = Path.Combine(tourPicsPath, picFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Tour.PicFile.CopyToAsync(stream, cancellationToken);
            }

            var updateDto = Tour.ToUpdateDto(picFileName);
            var result = await _tourService.UpdateAsync(Tour.TourId, updateDto, cancellationToken);

            if (result == null) return NotFound();

            TempData["SuccessMessage"] = "Tour updated successfully!";
            return RedirectToPage("./Details", new { id = Tour.TourId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID {TourId}", Tour.TourId);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the tour. Please try again.");
            return Page();
        }
    }
}
