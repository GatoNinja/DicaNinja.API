using DicaNinja.API.Enums;
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IHintProvider
{
    Task<Book?> GetHintAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> SetHintAccepted(Guid userId, Guid bookId, EnumHintStatus hintStatus, CancellationToken cancellationToken);
}
