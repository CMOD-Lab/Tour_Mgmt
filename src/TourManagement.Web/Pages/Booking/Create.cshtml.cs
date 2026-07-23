using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Booking;

/// <summary>
/// Booking create page model - migrated from Order.aspx.
/// </summary>
public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<CreateModel> _logger;

    [BindProperty]
    public BookingFormViewModel Input { get; set; } = new();

    public TourViewModel? SelectedTour { get; set; }
    public string? ErrorMessage { get; set; }

    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int? tourId, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/User/Login");
        }

        // Pre-fill user email and name from session
        Input.Email = email;
        Input.FirstName = HttpContext.Session.GetString("UserFirstName") ?? string.Empty;

        if (tourId.HasValue)
        {
            try
            {
                var tourDto = await _tourService.GetByIdAsync(tourId.Value, cancellationToken);
                if (tourDto != null)
                {
                    SelectedTour = new TourViewModel
                    {
                        TourId = tourDto.TourId,
                        TourName = tourDto.TourName,
                        Place = tourDto.Place,
                        Days = tourDto.Days,
                        Price = tourDto.Price,
                        Locations = tourDto.Locations,
                        TourInfo = tourDto.TourInfo,
                        Pic = tourDto.Pic
                    };

                    Input.TourName = tourDto.TourName;
                    Input.Place = tourDto.Place;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tour for booking, ID: {TourId}", tourId);
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/User/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manual mapping from ViewModel to DTO
            var createDto = new BookingCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName
            };

            await _bookingService.CreateAsync(createDto, cancellationToken);
            _logger.LogInformation("Booking created for user: {Email}, tour: {TourName}", Input.Email, Input.TourName);
            return RedirectToPage("/Booking/MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for user: {Email}", Input.Email);
            ErrorMessage = "An error occurred while creating the booking. Please try again.";
            return Page();
        }
    }
}
