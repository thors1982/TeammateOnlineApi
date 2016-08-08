using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeammateOnlineApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    GamePlatformId = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    FacebookId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    GoogleId = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FriendUserProfileId = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_UserProfiles_FriendUserProfileId",
                        column: x => x.FriendUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FriendUserProfileId = table.Column<int>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsPending = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    UserProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_UserProfiles_FriendUserProfileId",
                        column: x => x.FriendUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendUserProfileId",
                table: "Friends",
                column: "FriendUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserProfileId",
                table: "Friends",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_FriendUserProfileId",
                table: "FriendRequests",
                column: "FriendUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_UserProfileId",
                table: "FriendRequests",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_GameAccounts_UserName",
                table: "GameAccounts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_GameAccounts_UserProfileId",
                table: "GameAccounts",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_EmailAddress",
                table: "UserProfiles",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_FacebookId",
                table: "UserProfiles",
                column: "FacebookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_GoogleId",
                table: "UserProfiles",
                column: "GoogleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "GameAccounts");

            migrationBuilder.DropTable(
                name: "GamePlatforms");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
