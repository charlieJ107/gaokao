using Microsoft.EntityFrameworkCore.Migrations;

namespace Gaokao.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnRank",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnScore",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Provice",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ControlLine",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provice = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlLine", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Provice = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    MajorType = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    IsTopUniversity = table.Column<bool>(nullable: false),
                    IsTopMajor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Score_Rank_Table",
                columns: table => new
                {
                    Score_Rank_Table_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Provice = table.Column<string>(maxLength: 20, nullable: false),
                    type = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score_Rank_Table", x => x.Score_Rank_Table_ID);
                });

            migrationBuilder.CreateTable(
                name: "Tag_of_School",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag_of_School", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    Level_Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Major_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Score_Rank_Items_National_Old",
                columns: table => new
                {
                    Score_Rank_Items_National_Old_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score_Rank_Table_ID = table.Column<int>(nullable: false),
                    type = table.Column<string>(maxLength: 50, nullable: true),
                    Score = table.Column<int>(nullable: false),
                    People = table.Column<int>(nullable: false),
                    TotalPeople = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    Score_Rank_Table_ID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score_Rank_Items_National_Old", x => x.Score_Rank_Items_National_Old_ID);
                    table.ForeignKey(
                        name: "FK_Score_Rank_Items_National_Old_Score_Rank_Table_Score_Rank_Table_ID1",
                        column: x => x.Score_Rank_Table_ID1,
                        principalTable: "Score_Rank_Table",
                        principalColumn: "Score_Rank_Table_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTag",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTag", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SchoolTag_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTag_Tag_of_School_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag_of_School",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enroll_Distribution",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Provice = table.Column<string>(nullable: true),
                    MajorId = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    People = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enroll_Distribution", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Enroll_Distribution_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enroll_Overview",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    MajorId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Provice = table.Column<string>(nullable: true),
                    Max_Score = table.Column<int>(nullable: false),
                    Min_Score = table.Column<int>(nullable: false),
                    Ave_Score = table.Column<int>(nullable: false),
                    Max_Rank = table.Column<int>(nullable: false),
                    Min_Rank = table.Column<int>(nullable: false),
                    Ave_Rank = table.Column<int>(nullable: false),
                    Max_Score_Delta = table.Column<int>(nullable: false),
                    Min_Score_Delta = table.Column<int>(nullable: false),
                    Ave_Score_Delta = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enroll_Overview", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Enroll_Overview_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enroll_Distribution_MajorId",
                table: "Enroll_Distribution",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Enroll_Overview_MajorId",
                table: "Enroll_Overview",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Major_SchoolId",
                table: "Major",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTag_SchoolId",
                table: "SchoolTag",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTag_TagId",
                table: "SchoolTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_Rank_Items_National_Old_Score_Rank_Table_ID1",
                table: "Score_Rank_Items_National_Old",
                column: "Score_Rank_Table_ID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlLine");

            migrationBuilder.DropTable(
                name: "Enroll_Distribution");

            migrationBuilder.DropTable(
                name: "Enroll_Overview");

            migrationBuilder.DropTable(
                name: "SchoolTag");

            migrationBuilder.DropTable(
                name: "Score_Rank_Items_National_Old");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Tag_of_School");

            migrationBuilder.DropTable(
                name: "Score_Rank_Table");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropColumn(
                name: "OwnRank",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnScore",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Provice",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "AspNetUsers");
        }
    }
}
