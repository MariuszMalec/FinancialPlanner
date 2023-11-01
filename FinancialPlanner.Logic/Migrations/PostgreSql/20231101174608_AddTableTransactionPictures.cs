using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialPlanner.Logic.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class AddTableTransactionPictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionPictureId",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionPictures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPictures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionPictureId",
                table: "Transactions",
                column: "TransactionPictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionPictures_TransactionPictureId",
                table: "Transactions",
                column: "TransactionPictureId",
                principalTable: "TransactionPictures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionPictures_TransactionPictureId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionPictures");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionPictureId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionPictureId",
                table: "Transactions");
        }
    }
}
