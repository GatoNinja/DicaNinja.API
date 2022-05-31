namespace BookSearch.API.Contexts.Configurations;

internal interface ISoftDelete
{
    DateTimeOffset? Deleted { get; }
}