using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TeammateOnlineApi.Database;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace TeammateOnlineApi.Migrations
{
    [DbContext(typeof(TeammateOnlineContext))]
    partial class TeammateOnlineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("TeammateOnlineApi.Models.GamePlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .Required();

                    b.Property<string>("Url")
                        .Required();

                    b.Key("Id");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .Required();

                    b.Property<string>("FirstName")
                        .Required();

                    b.Property<string>("LastName")
                        .Required();

                    b.Key("Id");
                });
        }
    }
}
