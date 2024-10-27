﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using projekt.src.Data;

#nullable disable

namespace projekt.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("projekt.src.Models.Orders.OrderedItems", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnnouncementId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("SelectedColor")
                        .HasColumnType("text");

                    b.Property<string>("SelectedSize")
                        .HasColumnType("text");

                    b.Property<Guid>("ShoppingCartId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderedItems");
                });

            modelBuilder.Entity("projekt.src.Models.Orders.Orders", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DeliveryMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("OrderedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrderingPerson")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("projekt.src.Models.Reviews.Reviews", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnnouncementId")
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("projekt.src.Models.SavedAnnouncements.SavedAnnouncements", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnnouncementId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "AnnouncementId");

                    b.ToTable("SavedAnnouncements");
                });

            modelBuilder.Entity("projekt.src.Models.ShoppingCart.ShoppingCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("projekt.src.Models.ShoppingCart.ShoppingCartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnnouncementId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("SelectedColor")
                        .HasColumnType("text");

                    b.Property<string>("SelectedSize")
                        .HasColumnType("text");

                    b.Property<Guid>("ShoppingCartId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("projekt.src.Models.Store.Announcement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("projekt.src.Models.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("PostCode")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("projekt.src.Models.Orders.OrderedItems", b =>
                {
                    b.HasOne("projekt.src.Models.Orders.Orders", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("projekt.src.Models.Orders.Orders", b =>
                {
                    b.OwnsMany("projekt.src.Models.Users.UserId", "OwnersId", b1 =>
                        {
                            b1.Property<Guid>("OrdersId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("OwnersId");

                            b1.HasKey("OrdersId", "Id");

                            b1.ToTable("UserId");

                            b1.WithOwner()
                                .HasForeignKey("OrdersId");
                        });

                    b.Navigation("OwnersId");
                });

            modelBuilder.Entity("projekt.src.Models.ShoppingCart.ShoppingCartItem", b =>
                {
                    b.HasOne("projekt.src.Models.ShoppingCart.ShoppingCart", null)
                        .WithMany("Items")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("projekt.src.Models.Store.Announcement", b =>
                {
                    b.OwnsOne("projekt.src.Models.Store.Item", "Item", b1 =>
                        {
                            b1.Property<Guid>("AnnouncementId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Amount")
                                .HasColumnType("integer");

                            b1.Property<string>("Categories")
                                .HasColumnType("text");

                            b1.Property<string>("ColorsAmount")
                                .HasColumnType("text");

                            b1.Property<string>("ColorsSizesAmounts")
                                .HasColumnType("text");

                            b1.Property<decimal>("Cost")
                                .HasColumnType("numeric");

                            b1.Property<string>("Description")
                                .HasMaxLength(5000)
                                .HasColumnType("character varying(5000)");

                            b1.Property<string>("Model_Brand")
                                .HasColumnType("text");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)");

                            b1.Property<DateTime?>("UpdatedAt")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("AnnouncementId");

                            b1.ToTable("Announcements");

                            b1.WithOwner()
                                .HasForeignKey("AnnouncementId");
                        });

                    b.Navigation("Item")
                        .IsRequired();
                });

            modelBuilder.Entity("projekt.src.Models.Orders.Orders", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("projekt.src.Models.ShoppingCart.ShoppingCart", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
