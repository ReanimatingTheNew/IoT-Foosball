using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class TableGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "Guid", c => c.Guid(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Tables", "Guid");
        }
    }
}