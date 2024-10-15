using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS_API_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intiRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedbyId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "LastUpdatedbyId",
                table: "Session");

            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "Session",
                newName: "InActiveDate");

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenId",
                table: "Session",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Session_RefreshTokenId",
                table: "Session",
                column: "RefreshTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_RefreshToken_RefreshTokenId",
                table: "Session",
                column: "RefreshTokenId",
                principalTable: "RefreshToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_RefreshToken_RefreshTokenId",
                table: "Session");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Session_RefreshTokenId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Session");

            migrationBuilder.RenameColumn(
                name: "InActiveDate",
                table: "Session",
                newName: "LastLogin");

            migrationBuilder.AddColumn<string>(
                name: "CreatedbyId",
                table: "Session",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedbyId",
                table: "Session",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
