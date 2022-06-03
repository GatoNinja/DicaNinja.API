
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record ForgotPasswordRequest([Required, EmailAddress] string Email);