namespace Backend.Common.Models;

public partial class CardFrontBackLink
{
    public Guid? FrontCardId { get; set; } = null!;

    public Guid? BackCardId { get; set; } = null!;

    public virtual Card BackCard { get; set; } = null!;

    public virtual Card FrontCard { get; set; } = null!;

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();
}
