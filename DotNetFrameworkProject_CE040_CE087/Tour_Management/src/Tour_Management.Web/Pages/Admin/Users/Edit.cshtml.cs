using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Users;

/// <summary>
/// Page model for editing a user (admin).
/// </summary>
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the user edit input model.</summary>
    [BindProperty]
    public UserEditViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EditModel"/> class.
    /// </summary>
    public EditModel(IUserService userService, ILogger<EditModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the user edit page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        var dto = await _userService.GetByIdAsync(id, cancellationToken);
        if (dto == null)
            return NotFound();

        Input = new UserEditViewModel
        {
            UserId = dto.UserId,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            IsActive = dto.IsActive
        };
        return Page();
    }

    /// <summary>Handles POST requests for the user edit form.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var updateDto = new UserUpdateDto
            {
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                Street = Input.Street,
                City = Input.City,
                State = Input.State,
                IsActive = Input.IsActive
            };

            await _userService.UpdateAsync(Input.UserId, updateDto, cancellationToken);
            _logger.LogInformation("Admin updated user id {UserId}", Input.UserId);
            TempData["Success"] = "User updated successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user id {UserId}", Input.UserId);
            TempData["Error"] = "An error occurred while updating the user.";
            return Page();
        }
    }
}
