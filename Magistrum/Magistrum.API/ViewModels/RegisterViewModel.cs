using System.ComponentModel.DataAnnotations;

namespace Magistrum.API.ViewModels;
public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    public required string Password { get; set; }

    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    public required string ConfirmPassword { get; set; }
}
