namespace Dictionary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Base : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dictionary",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SourceId = c.Long(nullable: false),
                        DestinationId = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.DestinationId)
                .ForeignKey("dbo.Language", t => t.SourceId)
                .Index(t => new { t.SourceId, t.DestinationId }, unique: true, name: "IX_DictionaryCode");
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.DictionaryEntry",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DictionaryId = c.Long(nullable: false),
                        Word = c.String(maxLength: 100),
                        Meaning = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dictionary", t => t.DictionaryId, cascadeDelete: true)
                .Index(t => t.DictionaryId)
                .Index(t => t.Word);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DictionaryEntry", "DictionaryId", "dbo.Dictionary");
            DropForeignKey("dbo.Dictionary", "SourceId", "dbo.Language");
            DropForeignKey("dbo.Dictionary", "DestinationId", "dbo.Language");
            DropIndex("dbo.DictionaryEntry", new[] { "Word" });
            DropIndex("dbo.DictionaryEntry", new[] { "DictionaryId" });
            DropIndex("dbo.Language", new[] { "Code" });
            DropIndex("dbo.Dictionary", "IX_DictionaryCode");
            DropTable("dbo.DictionaryEntry");
            DropTable("dbo.Language");
            DropTable("dbo.Dictionary");
        }
    }
}
