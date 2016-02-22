using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class AddAccountEmailToPlayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "AccountEmail", c => c.String(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Players", "AccountEmail");
        }
    }
}