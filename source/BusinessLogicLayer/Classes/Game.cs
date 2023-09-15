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

    public Player Host { get; private set; }

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
            if (!Players.Any())
            {
                Host = player;
            }

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

    private void PlayerHandToDeck(Player player)
    {
        foreach (Card card in new List<Card>(player.Hand))
        {
            Deck.Add(card);
            player.RemoveCardFromHand(card);
        }

        ShuffleDeck();
    }

    // System input actions
    public StatusMessage Start()
    {
        if (_gameHasStarted)
        {
            return new StatusMessage(false, Messages.GameAlreadyStarted);
        }

        if (Players.Count < MinAmountOfPlayer)
        {
            return new StatusMessage(false, Messages.MinimumPlayersNotReached);
        }

        _gameHasStarted = true;
        InitializeDeck();
        DealCardsToPlayers();

        CurrentSet = new Set(Players);

        return new StatusMessage(true);
    }

    // Player input actions
    public StatusMessage DirtyLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Messages.PlayerNotFound);
        }

        return CurrentSet!.PlayerCallsDirtyLaundry(player);
    }

    public StatusMessage WhiteLaundry(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Messages.PlayerNotFound);
        }

        return CurrentSet!.PlayerCallsWhiteLaundry(player);
    }

    public StatusMessage TurnsLaundry(int playerId, int victimId)
    {
        if (playerId == victimId)
        {
            return new StatusMessage(false, Messages.CantDoThisActionOnYourself);
        }

        Player? player = Players.Find(p => p.Id == playerId);
        Player? victim = Players.Find(p => p.Id == victimId);

        if (player == null || victim == null)
        {
            return new StatusMessage(false, Messages.PlayerNotFound);
        }

        StatusMessage statusMessage = CurrentSet!.TurnsLaundry(player, victim);
        if (statusMessage.Message == Messages.PlayerDidNotBluff)
        {
            PlayerHandToDeck(victim);
            DealCardsToPlayer(victim);
        }

        return statusMessage;
    }

    public bool StopLaundryTurnTimerAndStartLaundryTimer()
    {
        foreach (Player player in Players)
        {
            if ((player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry) && !player.LaundryHasBeenTurned)
            {
                PlayerHandToDeck(player);
                DealCardsToPlayer(player);
            }

            player.ResetLaundryVariables();
        }

        return CurrentSet!.StopLaundryTurnTimerAndStartLaundryTimer();
    }

    public bool StopLaundryTurnTimerAndStartRound()
    {
        foreach (Player player in Players)
        {
            if ((player.HasCalledDirtyLaundry || player.HasCalledWhiteLaundry) && !player.LaundryHasBeenTurned)
            {
                PlayerHandToDeck(player);
                DealCardsToPlayer(player);
            }

            player.ResetLaundryVariables();
        }

        return CurrentSet!.StopLaundryTurnTimerAndStartRound();
    }

    public bool StopLaundryTimer()
    {
        return CurrentSet!.StopLaundryTimer();
    }

    public StatusMessage Knock(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Messages.PlayerNotFound);
        }

        return CurrentSet!.CurrentRound.Knock(player);
    }

    public StatusMessage Check(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Messages.PlayerNotFound);
        }

        return CurrentSet!.CurrentRound.Check(player);
    }

    public StatusMessage Fold(int playerId)
    {
        Player? player = Players.Find(p => p.Id == playerId);
        if (player == null)
        {
            return new StatusMessage(false, Messages.PlayerNotFound);
        }

        return CurrentSet!.CurrentRound.Fold(player);
    }
    
    //TODO: playcard
}