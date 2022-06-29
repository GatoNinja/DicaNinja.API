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

    public async Task<IEnumerable<BookResponse>> QueryBooksAsync(string query, CancellationToken cancellationToken)
    {
        var request = Service.Volumes.List(query);
        var response = await request.ExecuteAsync(cancellationToken);
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

    public async Task<Book?> CreateBookFromGoogleAsync(string identifier, CancellationToken cancellationToken)
    {
        var bookResponse = await GetFromIdentifierAsync(identifier, cancellationToken);

        if (bookResponse is null)
        {
            return null;
        }

        var book = await CreateFromResponse(bookResponse, cancellationToken);

        return book;
    }

    public async Task<BookResponse?> GetFromIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        var request = Service.Volumes.List(identifier);
        var response = await request.ExecuteAsync(cancellationToken);

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

    public async Task<Book?> CreateFromResponse(BookResponse response, CancellationToken cancellationToken)
    {
        var book = Mapper.Map<Book>(response);

        //book.Identifiers.RemoveAll(identifier => identifier.Id == Guid.Empty);
        //book.Authors.RemoveAll(identifier => identifier.Id == Guid.Empty);
        //book.Categories.RemoveAll(identifier => identifier.Id == Guid.Empty);

        //foreach (var identifier in response.Identifiers)
        //{
        //    var bookIdentifier = await IdentifierProvider.GetOrCreateAsync(identifier, cancellationToken);

        //    if (bookIdentifier is null)
        //    {
        //        continue;
        //    }

        //    book.Identifiers.Add(bookIdentifier);
        //}

        //foreach (var author in response.Authors)
        //{
        //    var authorEntity = await AuthorProvider.GetOrCreateAsync(author, cancellationToken);

        //    if (authorEntity is null)
        //    {
        //        continue;
        //    }

        //    book.Authors.Add(authorEntity);
        //}

        //foreach (var category in response.Categories)
        //{
        //    var categoryEntity = await CategoryProvider.GetOrCreateAsync(category, cancellationToken);

        //    if (categoryEntity is null)
        //    {
        //        continue;
        //    }

        //    book.Categories.Add(categoryEntity);
        //}

        Context.Books.Add(book);

        await Context.SaveChangesAsync(cancellationToken);

        return book;
    }
}
