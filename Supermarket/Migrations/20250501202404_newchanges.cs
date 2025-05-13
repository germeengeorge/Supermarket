using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supermarket.Migrations
{
    /// <inheritdoc />
    public partial class newchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "OrderHeaders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
