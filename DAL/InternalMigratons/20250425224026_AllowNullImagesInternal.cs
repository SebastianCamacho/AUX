using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class AllowNullImagesInternal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "image_Soport",
                table: "Document_Drivers",
                type: "LONGTEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "LONGTEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Document_Drivers",
                keyColumn: "image_Soport",
                keyValue: null,
                column: "image_Soport",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "image_Soport",
                table: "Document_Drivers",
                type: "LONGTEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "LONGTEXT",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
