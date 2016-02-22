using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class IntroduceTableSide : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Goals", "Side", c => c.Int(false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Goals", "Side", c => c.Byte(false));
        }
    }
}