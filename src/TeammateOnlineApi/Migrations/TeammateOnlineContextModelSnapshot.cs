using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TeammateOnlineApi.Database;

namespace TeammateOnlineApi.Migrations
{
    [DbContext(typeof(TeammateOnlineContext))]
    partial class TeammateOnlineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TeammateOnlineApi.Models.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("FriendUserProfileId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.GameAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("GamePlatformId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<int>("UserProfileId");

                    b.HasKey("Id");

                    b.ToTable("GameAccounts");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.GamePlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("GamePlatforms");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FacebookId");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("GoogleId");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.Friend", b =>
                {
                    b.HasOne("TeammateOnlineApi.Models.UserProfile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
