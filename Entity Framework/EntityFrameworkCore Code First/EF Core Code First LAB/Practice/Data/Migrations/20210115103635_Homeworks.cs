using Microsoft.EntityFrameworkCore.Migrations;

namespace Practice.Data.Migrations
{
    public partial class Homeworks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homework_Courses_CourseId",
                table: "Homework");

            migrationBuilder.DropForeignKey(
                name: "FK_Homework_Students_StudentId",
                table: "Homework");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homework",
                table: "Homework");

            migrationBuilder.RenameTable(
                name: "Homework",
                newName: "Homeworks");

            migrationBuilder.RenameIndex(
                name: "IX_Homework_StudentId",
                table: "Homeworks",
                newName: "IX_Homeworks_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Homework_CourseId",
                table: "Homeworks",
                newName: "IX_Homeworks_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homeworks",
                table: "Homeworks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Courses_CourseId",
                table: "Homeworks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Students_StudentId",
                table: "Homeworks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Courses_CourseId",
                table: "Homeworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Students_StudentId",
                table: "Homeworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homeworks",
                table: "Homeworks");

            migrationBuilder.RenameTable(
                name: "Homeworks",
                newName: "Homework");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_StudentId",
                table: "Homework",
                newName: "IX_Homework_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_CourseId",
                table: "Homework",
                newName: "IX_Homework_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homework",
                table: "Homework",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_Courses_CourseId",
                table: "Homework",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_Students_StudentId",
                table: "Homework",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
