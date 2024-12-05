using CMS_API_Core.DomainModels;
using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.DomainModels.Authentication;
using CMS_API_Core.DomainModels.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CMS_API_Infrastructure.DBcontext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);


            ///////////////// One-to-One /////////////////

            // One-to-One: User-Session 
            modelBuilder.Entity<User>()
                .HasOne(e => e.Session)
                .WithOne(e => e.User)
                .HasForeignKey<Session>(e => e.UserId)
                .IsRequired();

            // One-to-One: Session-RefreshToken 
            modelBuilder.Entity<RefreshToken>()
                .HasOne(e => e.Session)
                .WithOne(e => e.RefreshToken)
                .HasForeignKey<RefreshToken>(e => e.SessionId)
                .IsRequired();





            ///////////////// One-to-Many /////////////////




            // self-referencing :One-to-Many Comment(Parent)-Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId);

            // One-to-Many: Article-Comment 

            modelBuilder.Entity<ContentArticle>()
           .HasMany(e => e.Comments)
           .WithOne(e => e.Article)
           .HasForeignKey(e => e.ArticleId)
           .IsRequired();

            // One-to-Many: Role-User 

            modelBuilder.Entity<User>()
           .HasOne(e => e.Role)
           .WithMany(e => e.Users)
           .HasForeignKey(e => e.RoleId)
           .OnDelete(DeleteBehavior.NoAction);


            ///////////////////////////// CreatedBy 


            // One-to-Many: User(CreatedBy)-User 

            modelBuilder.Entity<User>()
           .HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedbyId)
           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(CreatedBy)-Role 

            modelBuilder.Entity<Role>()
           .HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedbyId)
           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(CreatedBy)-Permission 

            modelBuilder.Entity<Permission>()
           .HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedbyId)
           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(CreatedBy)-Article

            modelBuilder.Entity<ContentArticle>()
                 .HasOne(e => e.CreatedBy)
                 .WithMany()
                 .HasForeignKey(e => e.CreatedbyId)
                 .OnDelete(DeleteBehavior.NoAction);



            // One-to-Many: User(CreatedBy)-Tags 

            modelBuilder.Entity<Tag>()
           .HasOne(e => e.CreatedBy)
           .WithMany()
           .HasForeignKey(e => e.CreatedbyId)

           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(CreatedBy)-Comment

            modelBuilder.Entity<Comment>()
                .HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedbyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);



            ///////////////////////////// LastUpdatedBy 


            // One-to-Many: User(LastUpdatedBy)-User 

            modelBuilder.Entity<User>()
           .HasOne(e => e.LastUpdatedBy)
           .WithMany()
           .HasForeignKey(e => e.LastUpdatedbyId)

           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(LastUpdatedBy)-Role 

            modelBuilder.Entity<Role>()
           .HasOne(e => e.LastUpdatedBy)
           .WithMany()
           .HasForeignKey(e => e.LastUpdatedbyId)
           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(LastUpdatedBy)-Permission 

            modelBuilder.Entity<Permission>()
           .HasOne(e => e.LastUpdatedBy)
           .WithMany()
           .HasForeignKey(e => e.LastUpdatedbyId)

           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(LastUpdatedBy)-Article

            modelBuilder.Entity<ContentArticle>()
                 .HasOne(e => e.LastUpdatedBy)
                 .WithMany()
                 .HasForeignKey(e => e.LastUpdatedbyId)

           .OnDelete(DeleteBehavior.NoAction);



            // One-to-Many: User(LastUpdatedBy)-Tags 

            modelBuilder.Entity<Tag>()
           .HasOne(e => e.LastUpdatedBy)
           .WithMany()
           .HasForeignKey(e => e.LastUpdatedbyId)
           .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many: User(LastUpdatedBy)-Comment

            modelBuilder.Entity<Comment>()
                .HasOne(e => e.LastUpdatedBy)
                .WithMany()
                .HasForeignKey(e => e.LastUpdatedbyId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);





            ///////////////// Many-to-Many /////////////////


            // Many-to-Many: Role-Permission through RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => rp.Id);

            modelBuilder.Entity<Role>()
                 .HasMany(rp => rp.Permission)
                 .WithMany(r => r.Role)
                 .UsingEntity<RolePermission>();

            // Many-to-Many: Tag-Articles through TageArticle

            modelBuilder.Entity<TagArticle>()
              .HasKey(rp => rp.Id);

            modelBuilder.Entity<ContentArticle>()
                 .HasMany(rp => rp.Tags)
                 .WithMany(r => r.Articles)
                 .UsingEntity<TagArticle>();

            // Many-to-Many: Article-Contributors through ArticleContributor

            modelBuilder.Entity<ArticleContributor>()
              .HasKey(rp => rp.Id);

            modelBuilder.Entity<ContentArticle>()
                 .HasMany(rp => rp.Contributors)
                 .WithMany(r => r.Articles)
                 .UsingEntity<ArticleContributor>();



        }



        ////////// User Context //////////
        public DbSet<User> User { get; set; }//
        public DbSet<Session> Session { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Role> Role { get; set; }//
        public DbSet<Permission> Permissions { get; set; }//
        public DbSet<RolePermission> RolePermission { get; set; }

        ////////// Article Context ////////// 

        public DbSet<ContentArticle> Article { get; set; }//
        public DbSet<Comment> Comment { get; set; }//
        public DbSet<Tag> Tag { get; set; }//
        public DbSet<TagArticle> TagArticle { get; set; }
        public DbSet<ArticleContributor> ArticleContributor { get; set; }

    }

}
