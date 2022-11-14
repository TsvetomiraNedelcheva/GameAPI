using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mapping.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceCurrency",
                table: "Games",
                newName: "Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Games",
                newName: "PriceCurrency");
        }
    }
}
