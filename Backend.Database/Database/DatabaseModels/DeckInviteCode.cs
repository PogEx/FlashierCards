using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Database.DatabaseModels;

[PrimaryKey("DeckId")]
public class DeckInviteCode
{
        public Guid DeckId { get; set; }

        public string? Code { get; set; }
        
        public DateTime DateTime { get; set; }

        public virtual Deck Deck { get; set; } = null!;
}