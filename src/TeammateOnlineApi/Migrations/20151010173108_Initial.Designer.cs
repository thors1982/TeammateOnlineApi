using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Models;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace TeammateOnlineApi.Migrations
{
    [DbContext(typeof(TeammateOnlineContext))]
    partial class Initial
    {
        public override string Id
        {
            get { return "20151010173108_Initial"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("TeammateOnlineApi.Models.GameService", b =>
                {
                    b.Property<int>("GameServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .Required();

                    b.Property<string>("Url")
                        .Required();

                    b.Key("GameServiceId");
                });

            modelBuilder.Entity("TeammateOnlineApi.Models.UserProfile", b =>
                {
                    b.Property<int>("UserProfileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .Required();

                    b.Property<string>("FirstName")
                        .Required();

                    b.Property<string>("LastName")
                        .Required();

                    b.Key("UserProfileId");
                });
        }
    }
}
