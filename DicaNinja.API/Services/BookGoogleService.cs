using AutoMapper;

using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;
using DicaNinja.API.Startup;

using Google.Apis.Books.v1;
using Google.Apis.Services;

namespace DicaNinja.API.Services;

public class BookGoogleService
{
    public BookGoogleService(ConfigurationReader config, IMapper mapper, IIdentifierProvider identifierProvider, IAuthorProvider authorProvider, ICategoryProvider categoryProvider, BaseContext context)
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = config.Google.ApiKey,
            ApplicationName = config.Google.Application,
        });

        Service = service;
        Mapper = mapper;
        IdentifierProvider = identifierProvider;
        AuthorProvider = authorProvider;
        CategoryProvider = categoryProvider;
        Context = context;
    }

    private IMapper Mapper { get; }
    private BooksService Service { get; }
    private IIdentifierProvider IdentifierProvider { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoryProvider { get; }
    private BaseContext Context { get; }

    public async Task<IEnumerable<BookResponse>> QueryBooks(string query)
    {
        var request = Service.Volumes.List(query);
        var response = await request.ExecuteAsync();
        var books = Mapper.Map<List<BookResponse>>(response.Items);

        foreach (var book in books)
        {
            var identifiers = response.Items.FirstOrDefault(i => i.VolumeInfo.Title == book.Title)?.VolumeInfo.IndustryIdentifiers;

            if (identifiers is null)
            {
                continue;
            }

            book.Identifiers = new List<IdentifierResponse>();

            foreach (var identifier in identifiers)
            {
                book.Identifiers.Add(new IdentifierResponse(identifier.Identifier, identifier.Type));
            }
        }

        return books;
    }

    public async Task<Book?> CreateBookFromGoogle(string identifier)
    {
        var bookResponse = await GetFromIdentifier(identifier);

        if (bookResponse is null)
        {
            return null;
        }

        var book = await CreateFromResponse(bookResponse);

        return book;
    }

    public async Task<BookResponse?> GetFromIdentifier(string identifier)
    {
        var request = Service.Volumes.List(identifier);
        var response = await request.ExecuteAsync();

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
        bookResponse.Identifiers = new List<IdentifierResponse>();

        foreach (var id in item.VolumeInfo.IndustryIdentifiers)
        {
            bookResponse.Identifiers.Add(new IdentifierResponse(id.Identifier, id.Type));
        }

        return bookResponse;
    }

    public async Task<Book?> CreateFromResponse(BookResponse response)
    {
        var book = Mapper.Map<Book>(response);

        book.Identifiers.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Authors.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Categories.RemoveAll(identifier => identifier.Id == Guid.Empty);

        foreach (var identifier in response.Identifiers)
        {
            var bookIdentifier = await IdentifierProvider.GetOrCreate(identifier);

            if (bookIdentifier is null)
            {
                continue;
            }

            book.Identifiers.Add(bookIdentifier);
        }

        foreach (var author in response.Authors)
        {
            var authorEntity = await AuthorProvider.GetOrCreate(author);

            if (authorEntity is null)
            {
                continue;
            }

            book.Authors.Add(authorEntity);
        }

        foreach (var category in response.Categories)
        {
            var categoryEntity = await CategoryProvider.GetOrCreate(category);

            if (categoryEntity is null)
            {
                continue;
            }

            book.Categories.Add(categoryEntity);
        }

        Context.Books.Add(book);

        await Context.SaveChangesAsync();

        return book;
    }
}
