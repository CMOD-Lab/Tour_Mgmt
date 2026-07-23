using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>Page model for the bookings index/list page.</summary>
public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IBookingService bookingService, ILogger<IndexModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var dtos = await _bookingService.GetAllAsync(cancellationToken);

            // Manual mapping from DTO to ViewModel
            Bookings = dtos.Select(dto => new BookingViewModel
            {
                BookingId = dto.BookingId,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                TourId = dto.TourId,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings index page");
            Bookings = Enumerable.Empty<BookingViewModel>();
        }
    }
}
