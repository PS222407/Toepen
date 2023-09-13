using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Game
{
    private const int AmountStartCardsPlayer = 4;
    
    private const int MaxAmountOfPlayers = 6;

    public GameStates GameState { get; private set; }

    public List<Player> Players { get; private set; } = new();

    private List<Card> Deck { get; set; } = new();

    public Set CurrentSet { get; private set; }

    private Player _playerWhoKnocked;

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
    
    // Player input actions
    public void DirtyLaundry(int playerId)
    {
        GameState = GameStates.PlayerCalledDirtyLaundry;
        _playerWhoKnocked = Players.Find(p => p.Id == playerId)!;
    }
    
    public void WhiteLaundry(int playerId)
    {
        GameState = GameStates.PlayerCalledWhiteLaundry;
        _playerWhoKnocked = Players.Find(p => p.Id == playerId)!;
    }

    public void TurnsLaundry(int playerId)
    {
        
    }

    public void Knock(int playerId)
    {
        
    }

    public void Check(int playerId)
    {
        
    }

    public void Fold(int playerId)
    {
        
    }
}