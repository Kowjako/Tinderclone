﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingAppAPI.Persistence.Data.Migrations
{
    public partial class AddedForgottenContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Messages");
        }
    }
}
