namespace Products.WebApi.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(),
                        PhotoData = c.Binary(),
                        PhotoWidth = c.Int(false),
                        PhotoHeight = c.Int(false),
                        Price = c.Double(false),
                        LastUpdated = c.DateTime(false)
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Product");
        }
    }
}
