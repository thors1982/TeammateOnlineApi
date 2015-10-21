using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace TeammateOnlineApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameAccount",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(isNullable: false),
                    GamePlatformId = table.Column<int>(isNullable: false),
                    ModifiedDate = table.Column<DateTime>(isNullable: false),
                    UserName = table.Column<string>(isNullable: false),
                    UserProfileId = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAccount", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(isNullable: false),
                    ModifiedDate = table.Column<DateTime>(isNullable: false),
                    Name = table.Column<string>(isNullable: false),
                    Url = table.Column<string>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(isNullable: false),
                    EmailAddress = table.Column<string>(isNullable: false),
                    FirstName = table.Column<string>(isNullable: false),
                    LastName = table.Column<string>(isNullable: false),
                    ModifiedDate = table.Column<DateTime>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("GameAccount");
            migrationBuilder.DropTable("GamePlatform");
            migrationBuilder.DropTable("UserProfile");
        }
    }
}
