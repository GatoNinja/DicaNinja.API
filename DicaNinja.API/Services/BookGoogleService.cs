using AutoMapper;

using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;
using DicaNinja.API.Startup;

using Google.Apis.Books.v1;
using Google.Apis.Services;

namespace DicaNinja.API.Services;

public class BookGoogleService
{
    public BookGoogleService(ConfigurationReader config, IMapper mapper, IBookProvider bookProvider)
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = config.Google.ApiKey,
            ApplicationName = config.Google.Application,
        });

        this.Service = service;
        this.Mapper = mapper;
        this.BookProvider = bookProvider;
    }

    private IMapper Mapper { get; }
    private IBookProvider BookProvider { get; }
    private BooksService Service { get; }

    public async Task<IEnumerable<BookResponse>> QueryBooks(string query)
    {
        var request = this.Service.Volumes.List(query);
        var response = await request.ExecuteAsync();
        var books = this.Mapper.Map<List<BookResponse>>(response.Items);

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
        var request = this.Service.Volumes.List(identifier);
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
        var bookResponse = this.Mapper.Map<BookResponse>(item);
        bookResponse.Identifiers = new List<IdentifierResponse>();

        foreach (var id in item.VolumeInfo.IndustryIdentifiers)
        {
            bookResponse.Identifiers.Add(new IdentifierResponse(id.Identifier, id.Type));
        }

        var book = await this.BookProvider.CreateFromResponse(bookResponse);



        return book;
    }
}
