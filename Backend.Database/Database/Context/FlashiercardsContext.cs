using Backend.Database.Database.Configs;
using Backend.Database.Database.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend.Database.Database.Context;

public class FlashiercardsContext : DbContext
{
    public FlashiercardsContext()
    {
    }

    public FlashiercardsContext(DbContextOptions<FlashiercardsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardType> CardTypes { get; set; }

    public virtual DbSet<Deck> Decks { get; set; }

    public virtual DbSet<DeckInviteCode> DeckInviteCodes { get; set; }

    public virtual DbSet<Folder> Folders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }
    
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

            entity.HasIndex(e => e.DeckId, "card_deck_deck_id_fk");

            entity.HasIndex(e => e.BackId, "card_pk").IsUnique();

            entity.HasIndex(e => e.UserId, "card_user_user_id_fk");

            entity.Property(e => e.CardId)
                .HasMaxLength(36)
                .HasColumnName("card_id");
            entity.Property(e => e.BackId)
                .HasMaxLength(36)
                .HasColumnName("back_card_id");
            entity.Property(e => e.DeckId)
                .HasMaxLength(36)
                .HasColumnName("deck_id");
            entity.Property(e => e.Text)
                .HasMaxLength(1023)
                .HasColumnName("text");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");

            entity.HasOne(d => d.BackCard).WithOne(p => p.FrontCard)
                .HasForeignKey<Card>(d => d.BackId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("card_card_card_id_fk");

            entity.HasOne(d => d.Deck).WithMany(p => p.Cards)
                .HasForeignKey(d => d.DeckId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("card_deck_deck_id_fk");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("card_content_card_types_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Cards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("card_user_user_id_fk");
        });

        modelBuilder.Entity<CardType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("card_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasData([
                new CardType { Id = 1, Type = "empty" },
                new CardType { Id = 2, Type = "text/plain" },
                new CardType { Id = 3, Type = "text/latex" },
                new CardType { Id = 4, Type = "image/png" },
                new CardType { Id = 5, Type = "image/svg" },
                new CardType { Id = 6, Type = "image/jpg" },
                new CardType { Id = 7, Type = "text/markdown" },
            ]);
        });

        modelBuilder.Entity<Deck>(entity =>
        {
            entity.HasKey(e => e.DeckId).HasName("PRIMARY");

            entity.ToTable("deck");

            entity.HasIndex(e => e.FolderId, "deck_folder_folder_id_fk");

            entity.HasIndex(e => e.UserId, "deck_user_user_id_fk");

            entity.Property(e => e.DeckId)
                .HasMaxLength(36)
                .HasColumnName("deck_id");
            entity.Property(e => e.DeckTitle)
                .HasMaxLength(255)
                .HasColumnName("deck_title");
            entity.Property(e => e.FolderId)
                .HasMaxLength(36)
                .HasColumnName("folder_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Folder).WithMany(p => p.Decks)
                .HasForeignKey(d => d.FolderId)
                .HasConstraintName("deck_folder_folder_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Decks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("deck_user_user_id_fk");
        });

        modelBuilder.Entity<DeckInviteCode>(entity =>
        {
            entity.HasKey(e => e.DeckId).HasName("PRIMARY");

            entity.ToTable("deck_invite_codes");

            entity.HasIndex(e => e.Code, "deck_invite_codes_pk").IsUnique();

            entity.Property(e => e.DeckId)
                .HasMaxLength(36)
                .HasColumnName("deck_id");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .HasColumnName("code");
            entity.Property(e => e.ExpiryTime)
                .HasColumnType("datetime")
                .HasColumnName("expiry_date");

            entity.HasOne(d => d.Deck).WithOne(p => p.DeckInviteCode)
                .HasForeignKey<DeckInviteCode>(d => d.DeckId)
                .HasConstraintName("deck_invite_codes_deck_deck_id_fk");
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
            entity.Property(e => e.ColorHex)
                .HasMaxLength(6)
                .HasColumnName("color_hex");
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

            entity.HasData(new Folder
            {
                FolderId = Guid.NewGuid(),
                ColorHex = "FFFFFF",
                IsRoot = true,
                Name = "Home",
                UserId = new Guid("e87f8052-cf90-43e4-900d-b75239d4b08f")
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
            
            entity.HasData(new User
            {
                UserId = new Guid("e87f8052-cf90-43e4-900d-b75239d4b08f"), 
                Name = "User", 
                PasswordHash = "", 
                Salt = ""
            });
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
