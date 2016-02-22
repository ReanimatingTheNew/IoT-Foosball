using System.Data.Entity.Migrations;

namespace Referee.Web.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Goals",
                c => new
                {
                    Id = c.Int(false, true),
                    Side = c.Byte(false),
                    Time = c.DateTime(false),
                    Match_Id = c.Int(false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Matches", t => t.Match_Id, true)
                .Index(t => t.Match_Id);

            CreateTable(
                "dbo.Matches",
                c => new
                {
                    Id = c.Int(false, true),
                    IsFinished = c.Boolean(false),
                    StartTime = c.DateTime(false),
                    EndTime = c.DateTime(),
                    BlueTeam_Id = c.Int(false),
                    RedTeam_Id = c.Int(false),
                    Table_Id = c.Int(false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.BlueTeam_Id)
                .ForeignKey("dbo.Teams", t => t.RedTeam_Id)
                .ForeignKey("dbo.Tables", t => t.Table_Id)
                .Index(t => t.BlueTeam_Id)
                .Index(t => t.RedTeam_Id)
                .Index(t => t.Table_Id);

            CreateTable(
                "dbo.Teams",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String(maxLength: 255),
                    FirstPlayer_Id = c.Int(false),
                    SecondPlayer_Id = c.Int()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.FirstPlayer_Id, true)
                .ForeignKey("dbo.Players", t => t.SecondPlayer_Id)
                .Index(t => t.FirstPlayer_Id)
                .Index(t => t.SecondPlayer_Id);

            CreateTable(
                "dbo.Players",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String(false, 255)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Tables",
                c => new
                {
                    Id = c.Int(false, true),
                    Location = c.String(false, 255),
                    Name = c.String(false, 255)
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Goals", "Match_Id", "dbo.Matches");
            DropForeignKey("dbo.Matches", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.Matches", "RedTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Matches", "BlueTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Teams", "SecondPlayer_Id", "dbo.Players");
            DropForeignKey("dbo.Teams", "FirstPlayer_Id", "dbo.Players");
            DropIndex("dbo.Teams", new[] {"SecondPlayer_Id"});
            DropIndex("dbo.Teams", new[] {"FirstPlayer_Id"});
            DropIndex("dbo.Matches", new[] {"Table_Id"});
            DropIndex("dbo.Matches", new[] {"RedTeam_Id"});
            DropIndex("dbo.Matches", new[] {"BlueTeam_Id"});
            DropIndex("dbo.Goals", new[] {"Match_Id"});
            DropTable("dbo.Tables");
            DropTable("dbo.Players");
            DropTable("dbo.Teams");
            DropTable("dbo.Matches");
            DropTable("dbo.Goals");
        }
    }
}