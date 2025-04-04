using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class Agregacion_Propietarios_Contratantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Third_Party",
                newName: "id_Third");

            migrationBuilder.CreateTable(
                name: "Contracting_Partys",
                columns: table => new
                {
                    id_ = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    signature_Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    third_Partyid_Third = table.Column<string>(type: "VARCHAR(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracting_Partys", x => x.id_);
                    table.ForeignKey(
                        name: "FK_Contracting_Partys_Third_Party_third_Partyid_Third",
                        column: x => x.third_Partyid_Third,
                        principalTable: "Third_Party",
                        principalColumn: "id_Third");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    id_ = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_Owner = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    third_Partyid_Third = table.Column<string>(type: "VARCHAR(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.id_);
                    table.ForeignKey(
                        name: "FK_Owners_Third_Party_third_Partyid_Third",
                        column: x => x.third_Partyid_Third,
                        principalTable: "Third_Party",
                        principalColumn: "id_Third");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Contracting_Partys_third_Partyid_Third",
                table: "Contracting_Partys",
                column: "third_Partyid_Third");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_third_Partyid_Third",
                table: "Owners",
                column: "third_Partyid_Third");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracting_Partys");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.RenameColumn(
                name: "id_Third",
                table: "Third_Party",
                newName: "id");
        }
    }
}
