using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class AddColorsToTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "RedTeamColor", c => c.Int(false));
            AddColumn("dbo.Tables", "BlueTeamColor", c => c.Int(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Tables", "BlueTeamColor");
            DropColumn("dbo.Tables", "RedTeamColor");
        }
    }
}