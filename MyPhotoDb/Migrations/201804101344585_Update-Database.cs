namespace MyPhotoDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PictureEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        PhotoTakenDate = c.Time(nullable: false, precision: 7),
                        Description = c.String(),
                        Format = c.Int(nullable: false),
                        Resolution_Id = c.Guid(),
                        AlbumEntity_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResolutionEntities", t => t.Resolution_Id)
                .ForeignKey("dbo.AlbumEntities", t => t.AlbumEntity_Id)
                .Index(t => t.Resolution_Id)
                .Index(t => t.AlbumEntity_Id);
            
            CreateTable(
                "dbo.PositionInPictureEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        PictureObject_Id = c.Guid(),
                        PictureEntity_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PictureObjectEntities", t => t.PictureObject_Id)
                .ForeignKey("dbo.PictureEntities", t => t.PictureEntity_Id)
                .Index(t => t.PictureObject_Id)
                .Index(t => t.PictureEntity_Id);
            
            CreateTable(
                "dbo.PictureObjectEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        PreviewFileName = c.String(),
                        Name = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResolutionEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PictureEntities", "AlbumEntity_Id", "dbo.AlbumEntities");
            DropForeignKey("dbo.PictureEntities", "Resolution_Id", "dbo.ResolutionEntities");
            DropForeignKey("dbo.PositionInPictureEntities", "PictureEntity_Id", "dbo.PictureEntities");
            DropForeignKey("dbo.PositionInPictureEntities", "PictureObject_Id", "dbo.PictureObjectEntities");
            DropIndex("dbo.PositionInPictureEntities", new[] { "PictureEntity_Id" });
            DropIndex("dbo.PositionInPictureEntities", new[] { "PictureObject_Id" });
            DropIndex("dbo.PictureEntities", new[] { "AlbumEntity_Id" });
            DropIndex("dbo.PictureEntities", new[] { "Resolution_Id" });
            DropTable("dbo.ResolutionEntities");
            DropTable("dbo.PictureObjectEntities");
            DropTable("dbo.PositionInPictureEntities");
            DropTable("dbo.PictureEntities");
            DropTable("dbo.AlbumEntities");
        }
    }
}
