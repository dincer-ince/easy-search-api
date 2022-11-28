﻿// <auto-generated />
using EasySearchApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EasySearchApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221126175805_newDataTypes")]
    partial class newDataTypes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EasySearchApi.Models.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfDocuments")
                        .HasColumnType("integer");

                    b.Property<int>("totalNumberOfWords")
                        .HasColumnType("integer");

                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Dictionaries");
                });

            modelBuilder.Entity("EasySearchApi.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("dictionaryId")
                        .HasColumnType("integer");

                    b.Property<int>("numberOfWords")
                        .HasColumnType("integer");

                    b.Property<string>("rawDocument")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("dictionaryId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("EasySearchApi.Models.DocumentWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("count")
                        .HasColumnType("integer");

                    b.Property<int>("documentId")
                        .HasColumnType("integer");

                    b.Property<int>("wordId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("documentId");

                    b.HasIndex("wordId");

                    b.ToTable("DocumentWord");
                });

            modelBuilder.Entity("EasySearchApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EasySearchApi.Models.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("term")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("EasySearchApi.Models.Dictionary", b =>
                {
                    b.HasOne("EasySearchApi.Models.User", "user")
                        .WithMany("dictionaries")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("EasySearchApi.Models.Document", b =>
                {
                    b.HasOne("EasySearchApi.Models.Dictionary", "dictionary")
                        .WithMany("documents")
                        .HasForeignKey("dictionaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("dictionary");
                });

            modelBuilder.Entity("EasySearchApi.Models.DocumentWord", b =>
                {
                    b.HasOne("EasySearchApi.Models.Document", "document")
                        .WithMany("words")
                        .HasForeignKey("documentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasySearchApi.Models.Word", "word")
                        .WithMany("documents")
                        .HasForeignKey("wordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("document");

                    b.Navigation("word");
                });

            modelBuilder.Entity("EasySearchApi.Models.Dictionary", b =>
                {
                    b.Navigation("documents");
                });

            modelBuilder.Entity("EasySearchApi.Models.Document", b =>
                {
                    b.Navigation("words");
                });

            modelBuilder.Entity("EasySearchApi.Models.User", b =>
                {
                    b.Navigation("dictionaries");
                });

            modelBuilder.Entity("EasySearchApi.Models.Word", b =>
                {
                    b.Navigation("documents");
                });
#pragma warning restore 612, 618
        }
    }
}
