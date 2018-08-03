﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Todo.Models;

namespace Todo.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Todo.Models.Checklist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NoteId");

                    b.Property<string>("list");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.ToTable("Checklist");
                });

            modelBuilder.Entity("Todo.Models.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NoteId");

                    b.Property<string>("TagName");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.ToTable("Lable");
                });

            modelBuilder.Entity("Todo.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Pinned");

                    b.Property<string>("Text");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Todo.Models.Checklist", b =>
                {
                    b.HasOne("Todo.Models.Note")
                        .WithMany("Checklist")
                        .HasForeignKey("NoteId");
                });

            modelBuilder.Entity("Todo.Models.Label", b =>
                {
                    b.HasOne("Todo.Models.Note")
                        .WithMany("Labels")
                        .HasForeignKey("NoteId");
                });
#pragma warning restore 612, 618
        }
    }
}
