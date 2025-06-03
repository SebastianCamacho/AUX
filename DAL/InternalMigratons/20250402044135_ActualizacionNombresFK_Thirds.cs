using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class ActualizacionNombresFK_Thirds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Partyid_Third",
                table: "Contracting_Partys");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Third_Party_third_Partyid_Third",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_third_Partyid_Third",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Contracting_Partys_third_Partyid_Third",
                table: "Contracting_Partys");

            migrationBuilder.DropColumn(
                name: "third_Partyid_Third",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "third_Partyid_Third",
                table: "Contracting_Partys");

            migrationBuilder.RenameColumn(
                name: "id_Third",
                table: "Third_Party",
                newName: "thirdParty_Id");

            migrationBuilder.AddColumn<string>(
                name: "third_Id",
                table: "Owners",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "third_Id",
                table: "Contracting_Partys",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_third_Id",
                table: "Owners",
                column: "third_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contracting_Partys_third_Id",
                table: "Contracting_Partys",
                column: "third_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Id",
                table: "Contracting_Partys",
                column: "third_Id",
                principalTable: "Third_Party",
                principalColumn: "thirdParty_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Third_Party_third_Id",
                table: "Owners",
                column: "third_Id",
                principalTable: "Third_Party",
                principalColumn: "thirdParty_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Id",
                table: "Contracting_Partys");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Third_Party_third_Id",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_third_Id",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Contracting_Partys_third_Id",
                table: "Contracting_Partys");

            migrationBuilder.DropColumn(
                name: "third_Id",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "third_Id",
                table: "Contracting_Partys");

            migrationBuilder.RenameColumn(
                name: "thirdParty_Id",
                table: "Third_Party",
                newName: "id_Third");

            migrationBuilder.AddColumn<string>(
                name: "third_Partyid_Third",
                table: "Owners",
                type: "VARCHAR(20)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "third_Partyid_Third",
                table: "Contracting_Partys",
                type: "VARCHAR(20)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_third_Partyid_Third",
                table: "Owners",
                column: "third_Partyid_Third");

            migrationBuilder.CreateIndex(
                name: "IX_Contracting_Partys_third_Partyid_Third",
                table: "Contracting_Partys",
                column: "third_Partyid_Third");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracting_Partys_Third_Party_third_Partyid_Third",
                table: "Contracting_Partys",
                column: "third_Partyid_Third",
                principalTable: "Third_Party",
                principalColumn: "id_Third");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Third_Party_third_Partyid_Third",
                table: "Owners",
                column: "third_Partyid_Third",
                principalTable: "Third_Party",
                principalColumn: "id_Third");
        }
    }
}
