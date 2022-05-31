using BookSearch.API.Abstracts;

namespace BookSearch.API.DDD.User;

public class UserController : ControllerHelper
{
    public UserController(IUserRepository userRepository)
    {

    }
}