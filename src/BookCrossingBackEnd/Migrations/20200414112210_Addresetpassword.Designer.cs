﻿// <auto-generated />
using System;
using Infrastructure.RDBMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookCrossingBackEnd.Migrations
{
    [DbContext(typeof(BookCrossingContext))]
    [Migration("20200414112210_Addresetpassword")]
    partial class Addresetpassword
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.RDBMS.Entities.ResetPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConfirmationNumber")
                        .IsRequired()
                        .HasColumnName("confirmation_number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ResetDate")
                        .HasColumnName("reset_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ResetPassword");
                });
#pragma warning restore 612, 618
        }
    }
}