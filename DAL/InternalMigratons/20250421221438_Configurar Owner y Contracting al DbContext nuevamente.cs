using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class ConfigurarOwneryContractingalDbContextnuevamente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Third_Party_third_Id", // Nombre exacto, revisalo si no coincide
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Id", // Igual, verificar el nombre real
                table: "Contracting_Partys");
            migrationBuilder.DropIndex(
                name: "IX_Owners_third_Id",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Contracting_Partys_third_Id",
                table: "Contracting_Partys");

            migrationBuilder.AlterColumn<string>(
                name: "web_Site",
                table: "Third_Party",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name_Company",
                table: "Third_Party",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "check_Digit",
                table: "Third_Party",
                type: "INT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Owners",
                keyColumn: "type_Owner",
                keyValue: null,
                column: "type_Owner",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "type_Owner",
                table: "Owners",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id_",
                table: "Owners",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "signature_Image",
                table: "Contracting_Partys",
                type: "LONGTEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id_",
                table: "Contracting_Partys",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Owners_third_Id",
                table: "Owners",
                column: "third_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracting_Partys_third_Id",
                table: "Contracting_Partys",
                column: "third_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Third_Party_third_Id",
                table: "Owners",
                column: "third_Id",
                principalTable: "Third_Party",
                principalColumn: "thirdParty_Id",
                onDelete: ReferentialAction.Cascade // o Restrict, depende tu caso
);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Id",
                table: "Contracting_Partys",
                column: "third_Id",
                principalTable: "Third_Party",
                principalColumn: "thirdParty_Id",
                onDelete: ReferentialAction.Cascade // o Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar las Foreign Keys (revertir relaciones 1:1)
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Third_Party_third_Id",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Id",
                table: "Contracting_Partys");

            // Eliminar los índices únicos
            migrationBuilder.DropIndex(
                name: "IX_Owners_third_Id",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Contracting_Partys_third_Id",
                table: "Contracting_Partys");

            // Revertir columnas a su estado anterior (tipos largos)
            migrationBuilder.AlterColumn<string>(
                name: "web_Site",
                table: "Third_Party",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name_Company",
                table: "Third_Party",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "check_Digit",
                table: "Third_Party",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type_Owner",
                table: "Owners",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id_",
                table: "Owners",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "signature_Image",
                table: "Contracting_Partys",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "LONGTEXT",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "id_",
                table: "Contracting_Partys",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // Restaurar los índices simples
            migrationBuilder.CreateIndex(
                name: "IX_Owners_third_Id",
                table: "Owners",
                column: "third_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contracting_Partys_third_Id",
                table: "Contracting_Partys",
                column: "third_Id");
        }



    }
}
