using AutoMapper;

using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Response;
using DicaNinja.API.Startup;

using Google.Apis.Books.v1;
using Google.Apis.Services;

using static Google.Apis.Books.v1.VolumesResource.ListRequest;

namespace DicaNinja.API.Services;

public class BookGoogleService
{
    public BookGoogleService(ConfigurationReader config, IMapper mapper, BaseContext context)
    {
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = config.Google.ApiKey,
            ApplicationName = config.Google.Application,
        });

        Service = service;
        Mapper = mapper;
        Context = context;
    }

    private IMapper Mapper { get; }
    private BooksService Service { get; }
    private BaseContext Context { get; }

    public async Task<List<BookResponse>> QueryBooksAsync(string query, CancellationToken cancellation, int page = 1, int perPage = 10, string? langRestrict = null)
    {
        var request = Service.Volumes.List($"intitle:{query}");

        request.MaxResults = perPage;
        request.StartIndex = (page - 1) * perPage;
        request.PrintType = PrintTypeEnum.BOOKS;

        if (langRestrict != null)
        {
            request.LangRestrict = langRestrict;
        }

        var response = await request.ExecuteAsync(cancellation).ConfigureAwait(false);
        var books = Mapper.Map<List<BookResponse>>(response.Items);

        foreach (var book in books)
        {
            var identifiers = response.Items.FirstOrDefault(i => i.VolumeInfo.Title == book.Title)?.VolumeInfo.IndustryIdentifiers;

            if (identifiers is null)
            {
                continue;
            }

            foreach (var identifier in identifiers)
            {
                book.Identifiers.Add(new IdentifierResponse(identifier.Identifier, identifier.Type));
            }
        }

        return books;
    }

    public async Task<Book?> CreateBookFromGoogleAsync(string identifier, CancellationToken cancellation)
    {
        var bookResponse = await GetFromIdentifierAsync(identifier, cancellation).ConfigureAwait(false);

        if (bookResponse is null)
        {
            return null;
        }

        var book = await CreateFromResponse(bookResponse, cancellation).ConfigureAwait(false);

        return book;
    }

    public async Task<BookResponse?> GetFromIdentifierAsync(string identifier, CancellationToken cancellation)
    {
        var request = Service.Volumes.List($"isbn:{identifier}");
        var response = await request.ExecuteAsync(cancellation).ConfigureAwait(false);

        if (response is null)
        {
            return null;
        }

        if (response.TotalItems == 0)
        {
            return null;
        }

        var item = response.Items.First();
        var bookResponse = Mapper.Map<BookResponse>(item);

        foreach (var id in item.VolumeInfo.IndustryIdentifiers)
        {
            bookResponse.Identifiers.Add(new IdentifierResponse(id.Identifier, id.Type));
        }

        return bookResponse;
    }

    public async Task<Book?> CreateFromResponse(BookResponse response, CancellationToken cancellation)
    {
        var book = Mapper.Map<Book>(response);

        Context.Books.Add(book);

        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return book;
    }
}
