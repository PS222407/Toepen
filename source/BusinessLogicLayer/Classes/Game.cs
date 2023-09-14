using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Classes;

public class Game
{
    private const int AmountStartCardsPlayer = 4;
    
    private const int MinAmountOfPlayer = 2;
    
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
            DealCardsToPlayer(player);
        }
    }

    private void DealCardsToPlayer(Player player)
    {
        for (int i = 0; i < AmountStartCardsPlayer; i++)
        {
            Card nextCard = Deck.First();
            player.DealCard(nextCard);
            Deck.Remove(nextCard);
        }
    }

    private void PlayerHandToDeck(Player victim)
    {
        foreach (Card card in victim.Hand)
        {
            Deck.Add(card);
            victim.RemoveCardFromHand(card);
        }
        ShuffleDeck();
    }
    
    // System input actions
    public StatusMessage Start()
    {
        if (_gameHasStarted)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.GameAlreadyStarted,
            };
        }
        if (Players.Count < MinAmountOfPlayer)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.MinimumPlayersNotReached,
            };
        }
        _gameHasStarted = true;
        InitializeDeck();
        DealCardsToPlayers();

        CurrentSet = new Set(Players);

        return new StatusMessage
        {
            Success = true,
        };
    }

    // Player input actions
    public StatusMessage DirtyLaundry(int playerId)
    {
        return CurrentSet.PlayerCallsDirtyLaundry(Players.Find(p => p.Id == playerId));
    }
    
    public StatusMessage WhiteLaundry(int playerId)
    {
        return CurrentSet.PlayerCallsWhiteLaundry(Players.Find(p => p.Id == playerId));
    }

    public StatusMessage TurnsLaundry(int playerId, int victimId)
    {
        if (playerId == victimId)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.CantDoThisActionOnYourself,
            };
        }
        Player? player = Players.Find(p => p.Id == playerId);
        Player? victim = Players.Find(p => p.Id == victimId);
        
        if (player == null || victim == null)
        {
            return new StatusMessage
            {
                Success = false,
                Message = Messages.PlayerNotFound,
            };
        }

        StatusMessage statusMessage = CurrentSet.TurnsLaundry(player, victim);
        if (statusMessage.Message == Messages.PlayerDidNotBluff)
        {
            PlayerHandToDeck(victim);
            DealCardsToPlayer(victim);
            victim.ResetLaundryVariables();
        }
        
        return statusMessage;
    }

    public bool StopLaundryTurnTimerAndStartLaundryTimer()
    {
        return CurrentSet.StopLaundryTurnTimerAndStartLaundryTimer();
    }

    public bool StopLaundryTurnTimerAndStartRound()
    {
        return CurrentSet.StopLaundryTurnTimerAndStartRound();
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

    public bool StopLaundryTimer()
    {
        return CurrentSet.StopLaundryTimer();
    }
}