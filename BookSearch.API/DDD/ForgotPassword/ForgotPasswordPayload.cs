
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.DDD.ForgotPassword;

public record ForgotPasswordPayload([Required, EmailAddress] string Email);