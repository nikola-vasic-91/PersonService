using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonService.Infrastructure.Migrations
{
    public partial class Initial_Setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PersonDb");

            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "PersonDb",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaAccounts",
                schema: "PersonDb",
                columns: table => new
                {
                    SocialMediaAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaAccounts", x => x.SocialMediaAccountId);
                });

            migrationBuilder.CreateTable(
                name: "SocialSkills",
                schema: "PersonDb",
                columns: table => new
                {
                    SocialSkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialSkills", x => x.SocialSkillId);
                    table.ForeignKey(
                        name: "FK_SocialSkills_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "PersonDb",
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonSocialMediaAccounts",
                schema: "PersonDb",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SocialMediaAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSocialMediaAccounts", x => new { x.PersonId, x.SocialMediaAccountId });
                    table.ForeignKey(
                        name: "FK_PersonSocialMediaAccounts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "PersonDb",
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonSocialMediaAccounts_SocialMediaAccounts_SocialMediaAccountId",
                        column: x => x.SocialMediaAccountId,
                        principalSchema: "PersonDb",
                        principalTable: "SocialMediaAccounts",
                        principalColumn: "SocialMediaAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonSocialMediaAccounts_SocialMediaAccountId",
                schema: "PersonDb",
                table: "PersonSocialMediaAccounts",
                column: "SocialMediaAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialSkills_PersonId",
                schema: "PersonDb",
                table: "SocialSkills",
                column: "PersonId");

            migrationBuilder
                .InsertData(
                    schema: "PersonDb",
                    table: "SocialMediaAccounts",
                    columns: new[] { "SocialMediaAccountId", "Type" },
                    values: new object[]
                    {
                        "7425e42b-09a2-42c1-9f58-0ede8ff036de",
                        "Facebook"
                    });

            migrationBuilder
                .InsertData(
                    schema: "PersonDb",
                    table: "SocialMediaAccounts",
                    columns: new[] { "SocialMediaAccountId", "Type" },
                    values: new object[]
                    {
                        "8284d5e8-86f9-453f-a0cd-38d2500734c8",
                        "LinkedIn"
                    });

            migrationBuilder
                .InsertData(
                    schema: "PersonDb",
                    table: "SocialMediaAccounts",
                    columns: new[] { "SocialMediaAccountId", "Type" },
                    values: new object[]
                    {
                        "ee49d702-4b3d-4892-9889-0e787627cfa1",
                        "Twitter"
                    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonSocialMediaAccounts",
                schema: "PersonDb");

            migrationBuilder.DropTable(
                name: "SocialSkills",
                schema: "PersonDb");

            migrationBuilder.DropTable(
                name: "SocialMediaAccounts",
                schema: "PersonDb");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "PersonDb");
        }
    }
}
