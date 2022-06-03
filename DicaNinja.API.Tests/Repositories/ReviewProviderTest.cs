//using DicaNinja.API.Models;
//using DicaNinja.API.Providers;
//using DicaNinja.API.Tests.Abstracts;

//namespace DicaNinja.API.Tests.Repositories;

//public class ReviewProviderTest : BaseTest
//{
//    public ReviewProviderTest(ReviewProvider reviewProvider): base()
//    {
//        this.ReviewProvider = reviewProvider;
//    }

//    public ReviewProvider ReviewProvider { get; }


//    [Test]
//    public async Task CreateReview()
//    {
//        var mock = this.Reviews.First();
//        var firstBook = this.Books.First();
//        var firstUser = this.Users.First();
//        var lastUser = this.Users.Last();

//        var reviewFromNoValidBook = new Review()
//        {
//            BookId = Guid.NewGuid(),
//            UserId = firstUser.Id,
//            Rating = mock.Rating,
//            Text = "Meu novo review lindo"
//        };

//        Assert.ThrowsAsync<Exception>(async () =>
//        {
//            await this.ReviewProvider.CreateReview(reviewFromNoValidBook);
//        });

//        var reviewFromNoValidUser = new Review()
//        {
//            BookId = firstBook.Id,
//            UserId = Guid.NewGuid(),
//            Rating = mock.Rating,
//            Text = "Meu novo review lindo"
//        };

//        Assert.ThrowsAsync<Exception>(async () =>
//        {
//            await this.ReviewProvider.CreateReview(reviewFromNoValidUser);
//        });

//        var reviewFromADuplicatedUserAndBook = new Review()
//        {
//            BookId = firstBook.Id,
//            UserId = firstUser.Id,
//            Rating = mock.Rating,
//            Text = "Meu novo review lindo"
//        };

//        Assert.ThrowsAsync<Exception>(async () =>
//        {
//            await this.ReviewProvider.CreateReview(reviewFromADuplicatedUserAndBook);
//        });

//        var validReview = new Review()
//        {
//            BookId = firstBook.Id,
//            UserId = lastUser.Id,
//            Rating = mock.Rating,
//            Text = "Meu novo review lindo"
//        };

//        var review = await this.ReviewProvider.CreateReview(validReview);

//       Assert.That(review, Is.Not.Empty);
//    }
//}
