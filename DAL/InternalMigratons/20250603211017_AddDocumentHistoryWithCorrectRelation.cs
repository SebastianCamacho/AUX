using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.InternalMigratons
{
    /// <inheritdoc />
    public partial class AddDocumentHistoryWithCorrectRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Document_Driver_Histories",
                columns: table => new
                {
                    DocHistoryId = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentAuditId = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActionTimestamp = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ActionType = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangedByUserId = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriverHistoryId_FK = table.Column<int>(type: "INT", nullable: false),
                    type_Document = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_validity = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    end_validity = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    image_Soport = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_Expirable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    driver_Id_Original = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document_Driver_Histories", x => x.DocHistoryId);
                    table.ForeignKey(
                        name: "FK_Document_Driver_Histories_Driver_Histories_DriverHistoryId_FK",
                        column: x => x.DriverHistoryId_FK,
                        principalTable: "Driver_Histories",
                        principalColumn: "HistoryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DocHist_DocAuditId",
                table: "Document_Driver_Histories",
                column: "DocumentAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_DocHist_DriverHistoryIdFK",
                table: "Document_Driver_Histories",
                column: "DriverHistoryId_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document_Driver_Histories");
        }
    }
}
