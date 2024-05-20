namespace Backend.Common.Models;

public partial class CardType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
