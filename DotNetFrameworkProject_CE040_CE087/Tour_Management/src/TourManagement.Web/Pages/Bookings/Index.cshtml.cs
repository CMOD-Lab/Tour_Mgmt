using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for the all bookings list page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of bookings.</summary>
    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    /// <summary>Gets or sets the search term.</summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(IBookingService bookingService, ILogger<IndexModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the bookings list page.
    /// </summary>
    public async Task OnGetAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        SearchTerm = searchTerm;
        try
        {
            var dtos = string.IsNullOrWhiteSpace(searchTerm)
                ? await _bookingService.GetAllAsync(cancellationToken)
                : await _bookingService.SearchAsync(searchTerm, cancellationToken);

            Bookings = dtos.Select(dto => new BookingViewModel
            {
                Id = dto.Id,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                BookingDate = dto.BookingDate,
                TourId = dto.TourId,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings list");
            TempData["ErrorMessage"] = "An error occurred while loading bookings.";
        }
    }
}
