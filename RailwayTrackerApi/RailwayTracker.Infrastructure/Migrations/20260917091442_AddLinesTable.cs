using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RailwayTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLinesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stations_Code",
                table: "Stations");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Trains",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "Trains",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "Stations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopOrder",
                table: "Stations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "Announcements",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TerminusA = table.Column<string>(type: "text", nullable: false),
                    TerminusB = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trains_LineId",
                table: "Trains",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_LineId",
                table: "Stations",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Lines_LineId",
                table: "Stations",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Lines_LineId",
                table: "Trains",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Lines_LineId",
                table: "Stations");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Lines_LineId",
                table: "Trains");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Trains_LineId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Stations_LineId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "StopOrder",
                table: "Stations");

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "Announcements",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Code",
                table: "Stations",
                column: "Code",
                unique: true);
        }
    }
}
