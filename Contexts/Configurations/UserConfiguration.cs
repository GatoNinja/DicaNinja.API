
using BookSearch.API.DDD.PasswordHasher;
using BookSearch.API.DDD.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookSearch.API.Contexts.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
    public UserConfiguration(IPasswordHasher passwordHasher)
    {
        PasswordHasher = passwordHasher;
    }

    private IPasswordHasher PasswordHasher { get; }

    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.HasData(new UserModel
        {
            Created = DateTimeOffset.Now,
            Updated = DateTimeOffset.Now,
            Email = "ygor@ygorlazaro.com",
            Password = PasswordHasher.Hash("ygor"),
            Id = Guid.NewGuid(),
            Username = "ygor"
        });
    }
}