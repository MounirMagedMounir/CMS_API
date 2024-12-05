using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS_API_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditArticleParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "isPublished", table: "Article", newName: "Status", schema: "dbo");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Article",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "Status", table: "Article", newName: "isPublished", schema: "dbo");
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Article",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
