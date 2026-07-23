using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>Page model for viewing the current user's bookings.</summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
            return RedirectToPage("/Users/Login");

        try
        {
            var dtos = await _bookingService.GetByEmailAsync(userEmail, cancellationToken);

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
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading my bookings for email: {Email}", userEmail);
            Bookings = Enumerable.Empty<BookingViewModel>();
            return Page();
        }
    }
}
