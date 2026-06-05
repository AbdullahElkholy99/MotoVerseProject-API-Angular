using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoVerse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class removecolumnstockinproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "Stock");
        }
    }
}
