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
    partial class Initial
    {
        public override string Id
        {
            get { return "20151021003308_Initial"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("TeammateOnlineApi.Models.GameAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("GamePlatformId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("UserName")
                        .Required();

                    b.Property<int>("UserProfileId");

                    b.Key("Id");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.GamePlatform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

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

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .Required();

                    b.Property<string>("FirstName")
                        .Required();

                    b.Property<string>("LastName")
                        .Required();

                    b.Property<DateTime>("ModifiedDate");

                    b.Key("Id");
                });
        }
    }
}
