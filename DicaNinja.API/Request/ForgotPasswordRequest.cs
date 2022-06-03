
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public record ForgotPasswordRequest([Required, EmailAddress] string Email);