using AutoMapper;

using BookSearch.API.Abstracts;
using BookSearch.API.DDD.Author;
using BookSearch.API.DDD.Category;
using BookSearch.API.DDD.Favorite;
using BookSearch.API.DDD.Identifier;
using BookSearch.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Book
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerHelper
    {

        public BookController(BookGoogleService service, IBookRepository bookRepository, IMapper mapper, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, IIdentifierRepository identifierRepository, IFavoriteRepository favoriteRepository)
        {
            Service = service;
            BookRepository = bookRepository;
            Mapper = mapper;
            AuthorRepository = authorRepository;
            CategoryRepository = categoryRepository;
            IdentifierRepository = identifierRepository;
            FavoriteRepository = favoriteRepository;
        }

        private BookGoogleService Service { get; }
        private IBookRepository BookRepository { get; }
        private IMapper Mapper { get; }
        public IAuthorRepository AuthorRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IIdentifierRepository IdentifierRepository { get; }
        public IFavoriteRepository FavoriteRepository { get; }

        [HttpGet]
        public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] string query)
        {
            var books = await Service.QueryBooks(query);

            return Ok(books);
        }

        [HttpGet("favorite")]
        public async Task<ActionResult<List<BookResponse>>> GetFavorites([FromQuery] Helpers.QueryString query)
        {
            var books = await BookRepository.GetFavorites(UserId, query.Page, query.PerPage);
            var totalFavorites = await FavoriteRepository.GetFavoritesCount(UserId);
            var mapped = Mapper.Map<List<BookResponse>>(books);
            var paginated = PaginationHelper.CreatePagedResponse<BookResponse>(mapped, query, totalFavorites);

            return Ok(paginated);
        }

        [HttpGet("{bookId}/author")]
        public async Task<ActionResult<List<Author.Author>>> GetAuthors([FromRoute] Guid bookId)
        {
            var authors = await AuthorRepository.GetByBook(bookId);

            return Ok(authors);
        }

        [HttpGet("{bookId}/identifier")]
        public async Task<ActionResult<List<Identifier.Identifier>>> GetIdentifiers([FromRoute] Guid bookId)
        {
            var identifiers = await IdentifierRepository.GetByBook(bookId);

            return Ok(identifiers);
        }

        [HttpGet("{bookId}/category")]
        public async Task<ActionResult<List<Category.Category>>> GetCategories([FromRoute] Guid bookId)
        {
            var categories = await CategoryRepository.GetByBook(bookId);

            return Ok(categories);
        }

    }
}
