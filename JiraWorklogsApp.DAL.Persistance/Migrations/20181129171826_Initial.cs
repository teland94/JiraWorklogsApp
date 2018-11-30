using Microsoft.EntityFrameworkCore.Migrations;

namespace JiraWorklogsApp.DAL.Persistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JiraConnections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    InstanceUrl = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    AuthToken = table.Column<string>(nullable: true),
                    TempoAuthToken = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JiraConnections", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JiraConnections");
        }
    }
}
