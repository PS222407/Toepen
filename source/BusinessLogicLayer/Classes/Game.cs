using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Game
{
    private const int AmountStartCardsPlayer = 4;
    
    private const int MaxAmountOfPlayers = 6;

    public List<Player> Players { get; private set; } = new();

    private List<Card> Deck { get; set; } = new();

    private void AddCard(Card card)
    {
        Deck.Add(card);
    }

    public bool AddPlayer(Player player)
    {
        if (Players.Count < MaxAmountOfPlayers)
        {
            Players.Add(player);
            return true;
        }

        return false;
    }

    public void Start()
    {
        InitializeDeck();
        DealCardsToPlayers();

        Set set = new Set();
        
    }

    private void ShuffleDeck()
    {
        Random rnd = new Random();
        List<Card> shuffledDeck = Deck.OrderBy(x => rnd.Next()).ToList();
        Deck = shuffledDeck;
    }

    private void InitializeDeck()
    {
        foreach (Suits suit in Enum.GetValues(typeof(Suits)))
        {
            foreach (Values value in Enum.GetValues(typeof(Values)))
            {
                Card card = new Card(suit, value);
                AddCard(card);
            }
        }
    }

    private void DealCardsToPlayers()
    {
        ShuffleDeck();

        foreach (Player player in Players)
        {
            for (int i = 0; i < AmountStartCardsPlayer; i++)
            {
                Card nextCard = Deck.First();
                player.DealCard(nextCard);
                Deck.Remove(nextCard);
            }   
        }
    }
}