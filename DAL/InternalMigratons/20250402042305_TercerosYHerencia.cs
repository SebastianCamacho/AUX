using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class TercerosYHerencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Third_Partys",
                table: "Third_Partys");

            migrationBuilder.RenameTable(
                name: "Third_Partys",
                newName: "Third_Party");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Third_Party",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "check_Digit",
                table: "Third_Party",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "create_At",
                table: "Third_Party",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "create_By",
                table: "Third_Party",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Third_Party",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "first_Last_Name",
                table: "Third_Party",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "first_Name",
                table: "Third_Party",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Third_Party",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "legal_Representative",
                table: "Third_Party",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name_Company",
                table: "Third_Party",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "Third_Party",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "second_Last_Name",
                table: "Third_Party",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "second_Name",
                table: "Third_Party",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "state",
                table: "Third_Party",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type_Id",
                table: "Third_Party",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "type_Third_Party",
                table: "Third_Party",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "update_At",
                table: "Third_Party",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "update_By",
                table: "Third_Party",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "web_Site",
                table: "Third_Party",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Third_Party",
                table: "Third_Party",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    street_Type = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    street_Number = table.Column<int>(type: "INT", nullable: true),
                    intersection_Number = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    property_Number = table.Column<int>(type: "INT", nullable: true),
                    neighborhood = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    zip_Code = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    municipality = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    third_Id = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.id);
                    table.ForeignKey(
                        name: "FK_Address_Third_Party_third_Id",
                        column: x => x.third_Id,
                        principalTable: "Third_Party",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Address_third_Id",
                table: "Address",
                column: "third_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Third_Party",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "check_Digit",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "create_At",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "create_By",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "first_Last_Name",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "first_Name",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "image",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "legal_Representative",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "name_Company",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "second_Last_Name",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "second_Name",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "type_Id",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "type_Third_Party",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "update_At",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "update_By",
                table: "Third_Party");

            migrationBuilder.DropColumn(
                name: "web_Site",
                table: "Third_Party");

            migrationBuilder.RenameTable(
                name: "Third_Party",
                newName: "Third_Partys");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Third_Partys",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Third_Partys",
                table: "Third_Partys",
                column: "id");
        }
    }
}
