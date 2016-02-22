using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class AddCreatorToMatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "Creator_Id", c => c.Int(false));
            CreateIndex("dbo.Matches", "Creator_Id");
            AddForeignKey("dbo.Matches", "Creator_Id", "dbo.Players", "Id", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Matches", "Creator_Id", "dbo.Players");
            DropIndex("dbo.Matches", new[] {"Creator_Id"});
            DropColumn("dbo.Matches", "Creator_Id");
        }
    }
}