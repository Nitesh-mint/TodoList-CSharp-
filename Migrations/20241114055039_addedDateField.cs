﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.Migrations
{
    /// <inheritdoc />
    public partial class addedDateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TodoItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "TodoItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "TodoItems");
        }
    }
}
