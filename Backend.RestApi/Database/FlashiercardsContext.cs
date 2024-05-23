using Backend.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.Database;

public class FlashiercardsContext : DbContext
{
    private readonly IConfiguration _configuration;

    public FlashiercardsContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public FlashiercardsContext(DbContextOptions<FlashiercardsContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardFrontBackLink> CardFrontBackLinks { get; set; }

    public virtual DbSet<CardType> CardTypes { get; set; }

    public virtual DbSet<Deck> Decks { get; set; }
    public virtual DbSet<Folder> Folders { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSetting> UserSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(_configuration.GetSection("ConnectionStrings")["mysqldb"], ServerVersion.Parse("8.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PRIMARY");

            entity.ToTable("card");

            entity.HasIndex(e => e.Type, "card_content_card_types_id_fk");

            entity.Property(e => e.CardId)
                .HasMaxLength(36)
                .HasColumnName("card_id");
            entity.Property(e => e.Text)
                .HasMaxLength(1023)
                .HasColumnName("text");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_content_card_types_id_fk");
        });

        modelBuilder.Entity<CardFrontBackLink>(entity =>
        {
            entity.HasKey(e => e.FrontCardId).HasName("PRIMARY");

            entity.ToTable("card_front_back_link");

            entity.HasIndex(e => e.BackCardId, "card_back_card_id_fk");

            entity.Property(e => e.FrontCardId)
                .HasMaxLength(36)
                .HasColumnName("front_card_id");
            entity.Property(e => e.BackCardId)
                .HasMaxLength(36)
                .HasColumnName("back_card_id");

            entity.HasOne(d => d.BackCard).WithMany(p => p.CardFrontBackLinkBackCards)
                .HasForeignKey(d => d.BackCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_back_card_id_fk");

            entity.HasOne(d => d.FrontCard).WithOne(p => p.CardFrontBackLinkFrontCard)
                .HasForeignKey<CardFrontBackLink>(d => d.FrontCardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_front_card_id_fk");
        });

        modelBuilder.Entity<CardType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("card_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Deck>(entity =>
        {
            entity.HasKey(e => e.DeckId).HasName("PRIMARY");

            entity.ToTable("deck");

            entity.Property(e => e.DeckId)
                .HasMaxLength(5)
                .HasColumnName("deck_id");
            entity.Property(e => e.DeckTitle)
                .HasMaxLength(255)
                .HasColumnName("deck_title");

            entity.HasMany(d => d.Cards).WithMany(p => p.Decks)
                .UsingEntity<Dictionary<string, object>>(
                    "DeckCardLink",
                    r => r.HasOne<CardFrontBackLink>().WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("deckcontent_card_cardID_fk"),
                    l => l.HasOne<Deck>().WithMany()
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("deckcontent_deck_deckID_fk"),
                    j =>
                    {
                        j.HasKey("DeckId", "CardId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("deck_card_link");
                        j.HasIndex(new[] { "CardId" }, "deckcontent_card_cardID_fk");
                        j.IndexerProperty<string>("DeckId")
                            .HasMaxLength(5)
                            .HasColumnName("deck_id");
                        j.IndexerProperty<Guid>("CardId")
                            .HasMaxLength(36)
                            .HasColumnName("card_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Decks)
                .UsingEntity<Dictionary<string, object>>(
                    "UserDeckLink",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("subscription_user_guid_fk"),
                    l => l.HasOne<Deck>().WithMany()
                        .HasForeignKey("DeckId")
                        .HasConstraintName("subscription_deck_deckID_fk"),
                    j =>
                    {
                        j.HasKey("DeckId", "UserId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("user_deck_link");
                        j.HasIndex(new[] { "UserId" }, "subscription_user_guid_fk");
                        j.IndexerProperty<string>("DeckId")
                            .HasMaxLength(5)
                            .HasColumnName("deck_id");
                        j.IndexerProperty<Guid>("UserId")
                            .HasMaxLength(36)
                            .HasColumnName("user_id");
                    });
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => e.FolderId).HasName("PRIMARY");

            entity.ToTable("folder");

            entity.HasIndex(e => e.ParentId, "folder_folder_folder_id_fk");

            entity.HasIndex(e => e.UserId, "folder_user_user_id_fk");

            entity.Property(e => e.FolderId)
                .HasMaxLength(36)
                .HasColumnName("folder_id");
            entity.Property(e => e.IsRoot)
                .HasColumnType("bit(1)")
                .HasColumnName("is_root");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ParentId)
                .HasMaxLength(36)
                .HasColumnName("parent_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("folder_folder_folder_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Folders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("folder_user_user_id_fk");

            entity.HasMany(d => d.Decks).WithMany(p => p.Folders)
                .UsingEntity<Dictionary<string, object>>(
                    "FolderDeckLink",
                    r => r.HasOne<Deck>().WithMany()
                        .HasForeignKey("DeckId")
                        .HasConstraintName("folder_deck_link_deck_deck_id_fk"),
                    l => l.HasOne<Folder>().WithMany()
                        .HasForeignKey("FolderId")
                        .HasConstraintName("folder_deck_link_folder_folder_id_fk"),
                    j =>
                    {
                        j.HasKey("FolderId", "DeckId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("folder_deck_link");
                        j.HasIndex(new[] { "DeckId" }, "folder_deck_link_deck_deck_id_fk");
                        j.IndexerProperty<Guid>("FolderId")
                            .HasMaxLength(36)
                            .HasColumnName("folder_id");
                        j.IndexerProperty<string>("DeckId")
                            .HasMaxLength(5)
                            .HasColumnName("deck_id");
                    });
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
            entity.HasIndex(user => user.Name).IsUnique();
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user_settings");

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");
            entity.Property(e => e.IsDark)
                .HasColumnType("bit(1)")
                .HasColumnName("is_dark");

            entity.HasOne(d => d.User).WithOne(p => p.UserSetting)
                .HasForeignKey<UserSetting>(d => d.UserId)
                .HasConstraintName("user_settings_user_guid_fk");
        });
    }
}
