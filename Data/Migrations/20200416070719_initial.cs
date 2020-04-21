using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOOKS",
                columns: table => new
                {
                    BOOKID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    BOOK_TITLE = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    PUBLISH_DATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    PAGES = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BOOKS_CONST1_PK", x => x.BOOKID);
                });

            migrationBuilder.CreateTable(
                name: "AUTHORS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    AuthorName = table.Column<string>(nullable: false),
                    BookTableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTHORS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AUTHORS_BOOKS_BookTableId",
                        column: x => x.BookTableId,
                        principalTable: "BOOKS",
                        principalColumn: "BOOKID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUTHORS_BookTableId",
                table: "AUTHORS",
                column: "BookTableId");

            migrationBuilder.CreateIndex(
                name: "BOOKS_CONST2_UK",
                table: "BOOKS",
                column: "BOOK_TITLE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BOOKS_CONST1_PK",
                table: "BOOKS",
                column: "BOOKID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUTHORS");

            migrationBuilder.DropTable(
                name: "BOOKS");
        }
    }
}
