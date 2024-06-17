using Backend.Common.Models.Cards;
using Backend.Common.Models.Decks;
using Backend.Common.Models.Folders;
using Backend.Common.Models.Users;
using Backend.Database.Database.DatabaseModels;

namespace Backend.RestApi.Helpers.Extensions;

public static class DtoTypeExtensions
{
    public static DeckDto ToDto(this Deck deck)
    {
        return new DeckDto
        {
            DeckId = deck.DeckId,
            Name = deck.DeckTitle,
            Cards = deck.Cards.Select(c => c.CardId).ToList()
        };
    }
    
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Name = user.Name
        };
    }
    
    public static FolderDto ToDto(this Folder folder)
    {
        return new FolderDto
        {
            FolderId = folder.FolderId,
            UserId = folder.UserId,
            IsRoot = folder.IsRoot,
            Name = folder.Name,
            ParentId = folder.ParentId,
            ChildrenIds = folder.Children.Select(f => f.FolderId),
            DeckIds = folder.Decks.Select(d => d.DeckId),
            IsEmpty = folder.Children.Count + folder.Decks.Count == 0
        };
    }
    
    public static CardDto ToDto(this Card card)
    {
        return new CardDto { };
    }
}