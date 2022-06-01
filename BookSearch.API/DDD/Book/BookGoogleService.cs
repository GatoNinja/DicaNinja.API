using AutoMapper;

using BookSearch.API.Startup;

using Google.Apis.Books.v1;
using Google.Apis.Services;

namespace BookSearch.API.DDD.Book;

public class BookGoogleService
{
    public BookGoogleService(ConfigurationReader config, IMapper mapper)
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = config.Google.ApiKey,
            ApplicationName = config.Google.Application,
        });

        Service = service;
        Mapper = mapper;
    }

    private IMapper Mapper { get; }
    private BooksService Service { get; }

    public async Task<IEnumerable<BookResponse>> QueryBooks(string query)
    {
        var request = Service.Volumes.List(query);
        var response = await request.ExecuteAsync();
        var books = Mapper.Map<List<BookResponse>>(response.Items);

        return books;
    }
}
