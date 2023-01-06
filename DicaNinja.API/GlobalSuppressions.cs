// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security", "CA3003:Review code for file path injection vulnerabilities", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Controllers.UploadController.UploadFile(Microsoft.AspNetCore.Http.IFormFile)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Extensions.StringExtensions.Clean(System.String)~System.String")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Providers.UserProvider.SearchAsync(System.Guid,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{DicaNinja.API.Models.User}}")]
[assembly: SuppressMessage("Globalization", "CA1311:Specify a culture or use an invariant version", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Providers.UserProvider.SearchAsync(System.Guid,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{DicaNinja.API.Models.User}}")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Providers.UserProvider.SearchAsync(System.Guid,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{DicaNinja.API.Models.User}}")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Providers.PasswordRecoveryProvider.InsertAsync(DicaNinja.API.Models.PasswordRecovery,System.Threading.CancellationToken)~System.Threading.Tasks.Task{DicaNinja.API.Models.PasswordRecovery}")]
[assembly: SuppressMessage("Security", "CA5379:Ensure Key Derivation Function algorithm is sufficiently strong", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Services.PasswordHasher.Hash(System.String)~System.String")]
[assembly: SuppressMessage("Security", "CA5379:Ensure Key Derivation Function algorithm is sufficiently strong", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Services.PasswordHasher.Check(System.String,System.String)~System.Boolean")]
[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Startup.RequestLoggingMiddleware.Invoke(Microsoft.AspNetCore.Http.HttpContext)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "<Pending>", Scope = "type", Target = "~T:DicaNinja.API.Validations.ValidateModelFilter")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Extensions.StringExtensions.Clean(System.String)~System.String")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Migrations.Initial.Up(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:DicaNinja.API.Migrations.Review.Up(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)")]
