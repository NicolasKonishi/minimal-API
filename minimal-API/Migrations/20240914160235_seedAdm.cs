﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_API.Migrations
{
    /// <inheritdoc />
    public partial class seedAdm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "adms",
                columns: new[] { "Id", "Email", "Perfil", "Senha" },
                values: new object[] { 1, "Adm@teste.com", "Adm", "123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "adms",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
