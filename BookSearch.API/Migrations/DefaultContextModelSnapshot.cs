﻿// <auto-generated />
using System;
using BookSearch.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookSearch.API.Migrations
{
    [DbContext(typeof(DefaultContext))]
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

            modelBuilder.Entity("BookSearch.API.DDD.Author.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_authors");

                    b.ToTable("authors", (string)null);
                });

            modelBuilder.Entity("BookSearch.API.DDD.Book.Book", b =>
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

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

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

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_books");

                    b.ToTable("books", (string)null);
                });

            modelBuilder.Entity("BookSearch.API.DDD.Category.Category", b =>
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

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("BookSearch.API.DDD.Favorite.Favorite", b =>
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

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_favorites");

                    b.HasIndex("BookId")
                        .HasDatabaseName("ix_favorites_book_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_favorites_user_id");

                    b.ToTable("favorites", (string)null);
                });

            modelBuilder.Entity("BookSearch.API.DDD.Identifier.Identifier", b =>
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

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isbn");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_identifiers");

                    b.HasIndex("BookId")
                        .HasDatabaseName("ix_identifiers_book_id");

                    b.ToTable("identifiers", (string)null);
                });

            modelBuilder.Entity("BookSearch.API.DDD.PasswordRecovery.PasswordRecovery", b =>
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

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<DateTimeOffset>("ExpireDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expire_date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTimeOffset>("Updated")
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

            modelBuilder.Entity("BookSearch.API.DDD.Person.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("first_name");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("last_name");

                    b.Property<DateTimeOffset>("Updated")
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

            modelBuilder.Entity("BookSearch.API.DDD.RefreshToken.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTimeOffset>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_expiry_time");

                    b.Property<DateTimeOffset>("Updated")
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

            modelBuilder.Entity("BookSearch.API.DDD.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("Deleted")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTimeOffset>("Updated")
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
                    b.HasOne("BookSearch.API.DDD.Author.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_book_authors_authors_id");

                    b.HasOne("BookSearch.API.DDD.Book.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_book_books_books_id");
                });

            modelBuilder.Entity("BookCategory", b =>
                {
                    b.HasOne("BookSearch.API.DDD.Book.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_book_category_books_books_id");

                    b.HasOne("BookSearch.API.DDD.Category.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_book_category_categories_categories_id");
                });

            modelBuilder.Entity("BookSearch.API.DDD.Favorite.Favorite", b =>
                {
                    b.HasOne("BookSearch.API.DDD.Book.Book", "Book")
                        .WithMany("Favorites")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_favorites_books_book_id");

                    b.HasOne("BookSearch.API.DDD.User.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_favorites_users_user_id");

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookSearch.API.DDD.Identifier.Identifier", b =>
                {
                    b.HasOne("BookSearch.API.DDD.Book.Book", "Book")
                        .WithMany("Identifiers")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_identifiers_books_book_id");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("BookSearch.API.DDD.PasswordRecovery.PasswordRecovery", b =>
                {
                    b.HasOne("BookSearch.API.DDD.User.User", "UserModel")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_password_recoveries_users_user_id");

                    b.Navigation("UserModel");
                });

            modelBuilder.Entity("BookSearch.API.DDD.Person.Person", b =>
                {
                    b.HasOne("BookSearch.API.DDD.User.User", "User")
                        .WithOne("PersonModel")
                        .HasForeignKey("BookSearch.API.DDD.Person.Person", "UserId")
                        .HasConstraintName("fk_people_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BookSearch.API.DDD.RefreshToken.RefreshToken", b =>
                {
                    b.HasOne("BookSearch.API.DDD.User.User", "UserModel")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_tokens_users_user_id");

                    b.Navigation("UserModel");
                });

            modelBuilder.Entity("BookSearch.API.DDD.Book.Book", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Identifiers");
                });

            modelBuilder.Entity("BookSearch.API.DDD.User.User", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("PersonModel")
                        .IsRequired();

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
