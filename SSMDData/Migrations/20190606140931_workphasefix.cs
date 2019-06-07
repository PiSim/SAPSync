using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class workphasefix : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actions",
                table: "WorkPhaseLabData");

            migrationBuilder.DropColumn(
                name: "Analysis",
                table: "WorkPhaseLabData");

            migrationBuilder.DropColumn(
                name: "NotesC",
                table: "WorkPhaseLabData");

            migrationBuilder.DropColumn(
                name: "NotesG",
                table: "WorkPhaseLabData");

            migrationBuilder.DropColumn(
                name: "NotesP",
                table: "WorkPhaseLabData");

            migrationBuilder.RenameColumn(
                name: "TrialScope",
                table: "WorkPhaseLabData",
                newName: "WorkPhase");

            migrationBuilder.RenameColumn(
                name: "NotesS",
                table: "WorkPhaseLabData",
                newName: "Notes");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkPhase",
                table: "WorkPhaseLabData",
                newName: "TrialScope");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "WorkPhaseLabData",
                newName: "NotesS");

            migrationBuilder.AddColumn<string>(
                name: "Actions",
                table: "WorkPhaseLabData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Analysis",
                table: "WorkPhaseLabData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotesC",
                table: "WorkPhaseLabData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotesG",
                table: "WorkPhaseLabData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotesP",
                table: "WorkPhaseLabData",
                nullable: true);
        }

        #endregion Methods
    }
}