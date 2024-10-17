using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParfumeExpressApi.Migrations
{
    /// <inheritdoc />
    public partial class PostModelImageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostImage",
                table: "Posts",
                newName: "PostImagePath");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostCreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostImagePath",
                table: "Posts",
                newName: "PostImage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostCreationTime",
                table: "Posts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
