using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShoppingProject.DataAccess.Migrations
{
    public partial class AddSPToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCoverType
                @Id Int
                AS
                SELECT * from CoverTypes");

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCoverType
                @Id int
                AS
                SELECT * from CoverTypes where Id=@id");

            migrationBuilder.Sql(@" CREATE PROCEDURE SP_CreateCoverType
                @Name Varchar(50)
                AS
                insert CoverTypes values(@Name)");

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_UpdateCoverType
                @id int,
                @Name varchar(50)
                AS
               update CoverTypes set Name=@name where id=@id");

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_DeleteCoverType
                @id int               
                AS
              delete from CoverTypes  where id=@id");           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
