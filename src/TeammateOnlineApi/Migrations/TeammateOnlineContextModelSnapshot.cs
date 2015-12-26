using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TeammateOnlineApi.Database;

namespace TeammateOnlineApi.Migrations
{
    [DbContext(typeof(TeammateOnlineContext))]
    partial class TeammateOnlineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
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
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedDate");

                    b.HasKey("Id");
                });
        }
    }
}
