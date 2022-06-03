using AutoMapper;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Response;
using BookSearch.API.Startup;

using Google.Apis.Books.v1;
using Google.Apis.Services;

namespace BookSearch.API.Services;

public class BookGoogleService
{
    public BookGoogleService(ConfigurationReader config, IMapper mapper, IBookRepository bookRepository)
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = config.Google.ApiKey,
            ApplicationName = config.Google.Application,
        });

        Service = service;
        Mapper = mapper;
        BookRepository = bookRepository;
    }

    private IMapper Mapper { get; }
    private IBookRepository BookRepository { get; }
    private BooksService Service { get; }

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

        var book = await BookRepository.CreateFromResponse(bookResponse);



        return book;
    }
}
