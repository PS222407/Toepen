using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Game
{
    private const int AmountStartCardsPlayer = 4;
    
    private const int MaxAmountOfPlayers = 6;
    
    private bool _gameHasStarted = false;

    public GameStates GameState { get; private set; }

    public List<Player> Players { get; private set; } = new();

    private List<Card> Deck { get; set; } = new();

    public Set? CurrentSet { get; private set; }
    
    private void AddCard(Card card)
    {
        Deck.Add(card);
    }

    public bool AddPlayer(Player player)
    {
        if (_gameHasStarted)
        {
            return false;
        }
        if (Players.Count < MaxAmountOfPlayers)
        {
            Players.Add(player);
            return true;
        }

        return false;
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
    
    // System input actions
    public bool Start()
    {
        if (_gameHasStarted)
        {
            return false;
        }
        _gameHasStarted = true;
        InitializeDeck();
        DealCardsToPlayers();

        CurrentSet = new Set(Players);

        return true;
    }

    // Player input actions
    public bool DirtyLaundry(int playerId)
    {
        return CurrentSet.PlayerCallsDirtyLaundry(Players.Find(p => p.Id == playerId));
    }
    
    public bool WhiteLaundry(int playerId)
    {
        return CurrentSet.PlayerCallsWhiteLaundry(Players.Find(p => p.Id == playerId));
    }

    public void TurnsLaundry(int playerId, int victimId)
    {
        CurrentSet.TurnsLaundry(Players.Find(p => p.Id == playerId), Players.Find(p => p.Id == victimId));
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

    public bool LaundryTimeIsUp()
    {
        return CurrentSet.LaundryTimeIsUp();
    }
}