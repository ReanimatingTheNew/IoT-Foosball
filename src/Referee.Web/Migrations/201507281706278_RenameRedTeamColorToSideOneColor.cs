using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class RenameRedTeamColorToSideOneColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "SideOneColor", c => c.Int(false));
            AddColumn("dbo.Tables", "SideTwoColor", c => c.Int(false));
            DropColumn("dbo.Tables", "RedTeamColor");
            DropColumn("dbo.Tables", "BlueTeamColor");
        }

        public override void Down()
        {
            AddColumn("dbo.Tables", "BlueTeamColor", c => c.Int(false));
            AddColumn("dbo.Tables", "RedTeamColor", c => c.Int(false));
            DropColumn("dbo.Tables", "SideTwoColor");
            DropColumn("dbo.Tables", "SideOneColor");
        }
    }
}