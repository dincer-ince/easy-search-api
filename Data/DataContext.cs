using Microsoft.EntityFrameworkCore;

namespace EasySearchApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentWord>()
              .HasOne(b => b.document)
              .WithMany(c => c.words)
              .HasForeignKey(b => b.documentId);

            modelBuilder.Entity<DocumentWord>()
                .HasOne(b => b.word)
                .WithMany(w => w.documents)
                .HasForeignKey(w => w.wordId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<DocumentWord> DocumentWord { get; set; }
    }
}
