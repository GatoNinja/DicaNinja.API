
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record ForgotPasswordPayload([Required, EmailAddress] string Email);
