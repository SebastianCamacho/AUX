using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class ConsolidateDriverInfoIntoInternal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverGlobalRecordId",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Drivers",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "first_Last_Name",
                table: "Drivers",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "first_Name",
                table: "Drivers",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Drivers",
                type: "LONGTEXT",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "Drivers",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "second_Last_Name",
                table: "Drivers",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "second_Name",
                table: "Drivers",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "type_Id",
                table: "Drivers",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "web_Site",
                table: "Drivers",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "first_Last_Name",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "first_Name",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "image",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "second_Last_Name",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "second_Name",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "type_Id",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "web_Site",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "DriverGlobalRecordId",
                table: "Drivers",
                type: "INT",
                nullable: false,
                defaultValue: 0);
        }
    }
}
