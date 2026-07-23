using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.User;

/// <summary>
/// User edit profile page model.
/// </summary>
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    [BindProperty]
    public UserEditViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public EditModel(IUserService userService, ILogger<EditModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/User/Login");
        }

        try
        {
            var userDto = await _userService.GetByEmailAsync(email, cancellationToken);
            if (userDto == null)
            {
                return RedirectToPage("/User/Login");
            }

            // Manual mapping from DTO to ViewModel
            Input = new UserEditViewModel
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Gender = userDto.Gender,
                Dob = userDto.Dob,
                Street = userDto.Street,
                City = userDto.City,
                State = userDto.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit profile for email: {Email}", email);
            return RedirectToPage("/Error");
        }
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
            var updateDto = new UserUpdateDto
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                Dob = Input.Dob,
                Street = Input.Street,
                City = Input.City,
                State = Input.State
            };

            await _userService.UpdateAsync(email, updateDto, cancellationToken);
            HttpContext.Session.SetString("UserFirstName", Input.FirstName);
            _logger.LogInformation("User profile updated: {Email}", email);
            return RedirectToPage("/User/Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for email: {Email}", email);
            ErrorMessage = "An error occurred while updating your profile. Please try again.";
            return Page();
        }
    }
}
