﻿// <auto-generated />
using System;
using DicaNinja.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DicaNinja.API.Migrations
{
    [DbContext(typeof(BaseContext))]
    partial class DefaultContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.Property<Guid>("AuthorsId")
                        .HasColumnType("uuid")
                        .HasColumnName("authors_id");

                    b.Property<Guid>("BooksId")
                        .HasColumnType("uuid")
                        .HasColumnName("books_id");

                    b.HasKey("AuthorsId", "BooksId")
                        .HasName("pk_author_book");

                    b.HasIndex("BooksId")
                        .HasDatabaseName("ix_author_book_books_id");

                    b.ToTable("author_book", (string)null);
                });

            modelBuilder.Entity("BookCategory", b =>
                {
                    b.Property<Guid>("BooksId")
                        .HasColumnType("uuid")
                        .HasColumnName("books_id");

                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uuid")
                        .HasColumnName("categories_id");

                    b.HasKey("BooksId", "CategoriesId")
                        .HasName("pk_book_category");

                    b.HasIndex("CategoriesId")
                        .HasDatabaseName("ix_book_category_categories_id");

                    b.ToTable("book_category", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_authors");

                    b.ToTable("authors", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<double?>("AverageRating")
                        .HasColumnType("double precision")
                        .HasColumnName("average_ratting");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Language")
                        .HasColumnType("text")
                        .HasColumnName("language");

                    b.Property<int?>("PageCount")
                        .HasColumnType("integer")
                        .HasColumnName("page_count");

                    b.Property<string>("PublicationDate")
                        .HasColumnType("text")
                        .HasColumnName("publication_date");

                    b.Property<string>("Publisher")
                        .HasColumnType("text")
                        .HasColumnName("publisher");

                    b.Property<string>("Subtitle")
                        .HasColumnType("text")
                        .HasColumnName("subtitle");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_books");

                    b.ToTable("books", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Bookmark", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bookmarks");

                    b.HasIndex("BookId")
                        .HasDatabaseName("ix_bookmarks_book_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_bookmarks_user_id");

                    b.ToTable("bookmarks", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Follower", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<Guid>("FollowedId")
                        .HasColumnType("uuid")
                        .HasColumnName("follower_id");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_followers");

                    b.HasIndex("FollowedId")
                        .HasDatabaseName("ix_followers_follower_id");

                    b.HasIndex("UserId", "FollowedId")
                        .IsUnique()
                        .HasDatabaseName("ix_followers_user_id_follower_id");

                    b.ToTable("followers", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Identifier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isbn");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_identifiers");

                    b.HasIndex("BookId")
                        .HasDatabaseName("ix_identifiers_book_id");

                    b.ToTable("identifiers", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.PasswordRecovery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("code");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset>("ExpireDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expire_date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_password_recoveries");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_password_recoveries_user_id");

                    b.ToTable("password_recoveries", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("last_name");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_people");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_people_user_id");

                    b.ToTable("people", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTimeOffset>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_expiry_time");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_refresh_tokens");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_tokens_user_id");

                    b.ToTable("refresh_tokens", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("text");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_reviews");

                    b.HasIndex("BookId")
                        .HasDatabaseName("ix_reviews_book_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.ToTable("reviews", (string)null);
                });

            modelBuilder.Entity("DicaNinja.API.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.HasOne("DicaNinja.API.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_book_authors_authors_id");

                    b.HasOne("DicaNinja.API.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_book_books_books_id");
                });

            modelBuilder.Entity("BookCategory", b =>
                {
                    b.HasOne("DicaNinja.API.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_book_category_books_books_id");

                    b.HasOne("DicaNinja.API.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_book_category_categories_categories_id");
                });

            modelBuilder.Entity("DicaNinja.API.Models.Bookmark", b =>
                {
                    b.HasOne("DicaNinja.API.Models.Book", "Book")
                        .WithMany("Bookmarks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bookmarks_books_book_id");

                    b.HasOne("DicaNinja.API.Models.User", "User")
                        .WithMany("Bookmarks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bookmarks_users_user_id");

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DicaNinja.API.Models.Follower", b =>
                {
                    b.HasOne("DicaNinja.API.Models.User", "User")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_followers_users_user_id1");

                    b.HasOne("DicaNinja.API.Models.User", "FollowedUser")
                        .WithMany("Following")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_followers_users_user_id");

                    b.Navigation("FollowedUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DicaNinja.API.Models.Identifier", b =>
                {
                    b.HasOne("DicaNinja.API.Models.Book", "Book")
                        .WithMany("Identifiers")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_identifiers_books_book_id");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("DicaNinja.API.Models.PasswordRecovery", b =>
                {
                    b.HasOne("DicaNinja.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_password_recoveries_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DicaNinja.API.Models.Person", b =>
                {
                    b.HasOne("DicaNinja.API.Models.User", "User")
                        .WithOne("Person")
                        .HasForeignKey("DicaNinja.API.Models.Person", "UserId")
                        .HasConstraintName("fk_people_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DicaNinja.API.Models.RefreshToken", b =>
                {
                    b.HasOne("DicaNinja.API.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DicaNinja.API.Models.Review", b =>
                {
                    b.HasOne("DicaNinja.API.Models.Book", null)
                        .WithMany("Reviews")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_books_book_id");

                    b.HasOne("DicaNinja.API.Models.User", null)
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_users_user_id");
                });

            modelBuilder.Entity("DicaNinja.API.Models.Book", b =>
                {
                    b.Navigation("Bookmarks");

                    b.Navigation("Identifiers");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("DicaNinja.API.Models.User", b =>
                {
                    b.Navigation("Bookmarks");

                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Person")
                        .IsRequired();

                    b.Navigation("RefreshTokens");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
