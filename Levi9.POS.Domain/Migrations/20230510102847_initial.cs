using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Levi9.POS.Domain.Migrations
{
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230510130506_InitialMigration.cs
    /// <inheritdoc />
    public partial class InitialMigration : Migration
========
    public partial class initial : Migration
>>>>>>>> 75b6e89089cc7e3bdcdff7d1c7b441cbe7c47637:Levi9.POS.Domain/Migrations/20230510102847_initial.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastUpdate = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreationDay = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDocuments",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDocuments", x => new { x.ProductId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_ProductDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDocuments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "GlobalId", "LastUpdate", "Name", "PasswordHash", "Phone", "Salt" },
                values: new object[,]
                {
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230510130506_InitialMigration.cs
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("489322bc-460b-4ffa-952d-b5ab81a41afd"), "133281975060342291", "Marko", "password", "+387 65 132 527", "Salt" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("ac497f15-59ef-4cf9-81ad-35b8880abbd6"), "133281975060342306", "Aleksa", "password123", "+387 64 862 476", "Salt123" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("ef0cef6e-7adc-4a9e-bf8e-203a8f7db718"), "133281975060342314", "Milos", "password123", "+387 65 912 127", "Salt123" }
========
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("5d03ccb9-7a09-49d0-bab3-8ff56aa11c22"), "133281881272337945", "Marko", "password", "+387 65 132 527", "Salt" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("abc07698-dfb1-4075-94a5-00f73d512c52"), "133281881272337986", "Aleksa", "password123", "+387 64 862 476", "Salt123" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("d1288461-7eea-43c3-bc2e-0e5eef6baa86"), "133281881272338028", "Milos", "password123", "+387 65 912 127", "Salt123" }
>>>>>>>> 75b6e89089cc7e3bdcdff7d1c7b441cbe7c47637:Levi9.POS.Domain/Migrations/20230510102847_initial.cs
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "GlobalId", "LastUpdate", "Name", "Price", "ProductImageUrl" },
                values: new object[,]
                {
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230510130506_InitialMigration.cs
                    { 1, 30, new Guid("5533268a-fbad-4817-ac83-cdb057d9fc01"), "133281975060342021", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("58e72cde-0802-447b-bf19-008017f4af69"), "133281975060342108", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("4535b569-e936-4120-9054-c6b301f9495e"), "133281975060342130", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
========
                    { 1, 30, new Guid("ef7d3675-151e-47ae-9923-3ea709693475"), "133281881272337420", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("45edb050-4476-4f2e-86fa-1015e969fcc7"), "133281881272337531", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("d12e1f4f-2ae9-4edd-8d75-60f08b3caeb0"), "133281881272337552", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
>>>>>>>> 75b6e89089cc7e3bdcdff7d1c7b441cbe7c47637:Levi9.POS.Domain/Migrations/20230510102847_initial.cs
                });

            migrationBuilder.InsertData(
                table: "Documents",
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230510130506_InitialMigration.cs
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[,]
                {
                    { 1, 1, "133281975060342338", "INVOICE", new Guid("976d48fd-e7ba-45f6-b578-02f612e810f5"), "133281975060342345" },
                    { 2, 2, "133281975060342354", "INVOICE", new Guid("721dd915-9975-410a-bfdd-51ca9d20280f"), "133281975060342359" },
                    { 3, 3, "133281975060342366", "INVOICE", new Guid("ff0c1315-4a19-4f1c-829f-27027902884a"), "133281975060342372" }
                });
========
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumentType", "GlobalId", "LastUpdate" },
                values: new object[] { 1, 1, "133281881272338083", "INVOICE", new Guid("d1a87fab-26f7-46d3-8201-ab713703ac11"), "133281881272338098" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumentType", "GlobalId", "LastUpdate" },
                values: new object[] { 2, 2, "133281881272338113", "INVOICE", new Guid("d572a803-ab28-49a0-b1f9-d163f2ad22ff"), "133281881272338126" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumentType", "GlobalId", "LastUpdate" },
                values: new object[] { 3, 3, "133281881272338141", "INVOICE", new Guid("e10be4e8-e414-43c8-82a2-514c740703ee"), "133281881272338153" });
>>>>>>>> 75b6e89089cc7e3bdcdff7d1c7b441cbe7c47637:Levi9.POS.Domain/Migrations/20230510102847_initial.cs

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "RSD", 1200f, 20 },
                    { 2, 2, "EUR", 10f, 10 },
                    { 3, 3, "USD", 15f, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ClientId",
                table: "Documents",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDocuments_DocumentId",
                table: "ProductDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDocuments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
