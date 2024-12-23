﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RestaurantManagement.Infrastructure.Database.Command;

#nullable disable

namespace RestaurantManagement.Infrastructure.Migrations
{
    [DbContext(typeof(CommandDbContext))]
    [Migration("20241223154608_IntegrateFoodInMenuItems")]
    partial class IntegrateFoodInMenuItems
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventBus.Models.EventLog", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("event_date");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("character varying(70)")
                        .HasColumnName("event_name");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<int>("TimesSent")
                        .HasColumnType("integer")
                        .HasColumnName("times_sent");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("topic");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.HasKey("EventId")
                        .HasName("pk_event_logs");

                    b.HasIndex("State", "TransactionId")
                        .HasDatabaseName("ix_event_logs_state_transaction_id");

                    b.ToTable("event_logs", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.FoodTypeAggregate.FoodType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_food_types");

                    b.ToTable("food_types", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.FoodTypeMenuItem", b =>
                {
                    b.Property<Guid>("FoodTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("food_type_id");

                    b.Property<Guid>("MenuItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("menu_item_id");

                    b.HasKey("FoodTypeId", "MenuItemId")
                        .HasName("pk_food_type_menu_item");

                    b.HasIndex("MenuItemId")
                        .HasDatabaseName("ix_food_type_menu_item_menu_item_id");

                    b.ToTable("food_type_menu_item", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid")
                        .HasColumnName("restaurant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id")
                        .HasName("pk_menus");

                    b.HasIndex("RestaurantId")
                        .HasDatabaseName("ix_menus_restaurant_id");

                    b.ToTable("menus", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.MenuItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid")
                        .HasColumnName("menu_id");

                    b.Property<long>("Stock")
                        .HasColumnType("bigint")
                        .HasColumnName("stock");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.ComplexProperty<Dictionary<string, object>>("Specification", "RestaurantManagement.Domain.Models.Command.MenuAggregate.MenuItem.Specification#FoodSpecification", b1 =>
                        {
                            b1.Property<string>("Description")
                                .HasColumnType("text")
                                .HasColumnName("specification_description");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("specification_name");

                            b1.Property<decimal>("Price")
                                .HasColumnType("numeric")
                                .HasColumnName("specification_price");
                        });

                    b.HasKey("Id")
                        .HasName("pk_menu_items");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_menu_items_category_id");

                    b.HasIndex("MenuId")
                        .HasDatabaseName("ix_menu_items_menu_id");

                    b.ToTable("menu_items", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate.MenuCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_menu_categories");

                    b.ToTable("menu_categories", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.RestaurantAggregate.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "RestaurantManagement.Domain.Models.Command.RestaurantAggregate.Restaurant.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_city");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_state");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_zip_code");
                        });

                    b.HasKey("Id")
                        .HasName("pk_restaurants");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_restaurants_owner_id");

                    b.ToTable("restaurants", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.RestaurantAggregate.RestaurantOwner", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_restaurant_owners");

                    b.ToTable("restaurant_owners", (string)null);
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.FoodTypeMenuItem", b =>
                {
                    b.HasOne("RestaurantManagement.Domain.Models.Command.FoodTypeAggregate.FoodType", null)
                        .WithMany()
                        .HasForeignKey("FoodTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_food_type_menu_item_food_types_food_type_id");

                    b.HasOne("RestaurantManagement.Domain.Models.Command.MenuAggregate.MenuItem", null)
                        .WithMany("FoodTypeMenuItems")
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_food_type_menu_item_menu_items_menu_item_id");
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.Menu", b =>
                {
                    b.HasOne("RestaurantManagement.Domain.Models.Command.RestaurantAggregate.Restaurant", null)
                        .WithMany()
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_menus_restaurants_restaurant_id");
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.MenuItem", b =>
                {
                    b.HasOne("RestaurantManagement.Domain.Models.Command.MenuCategoryAggregate.MenuCategory", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_menu_items_menu_categories_category_id");

                    b.HasOne("RestaurantManagement.Domain.Models.Command.MenuAggregate.Menu", null)
                        .WithMany("MenuItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_menu_items_menus_menu_id");
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.RestaurantAggregate.Restaurant", b =>
                {
                    b.HasOne("RestaurantManagement.Domain.Models.Command.RestaurantAggregate.RestaurantOwner", null)
                        .WithMany("Restaurants")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_restaurants_restaurant_owners_owner_id");
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.Menu", b =>
                {
                    b.Navigation("MenuItems");
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.MenuAggregate.MenuItem", b =>
                {
                    b.Navigation("FoodTypeMenuItems");
                });

            modelBuilder.Entity("RestaurantManagement.Domain.Models.Command.RestaurantAggregate.RestaurantOwner", b =>
                {
                    b.Navigation("Restaurants");
                });
#pragma warning restore 612, 618
        }
    }
}
