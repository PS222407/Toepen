﻿using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.States;

public interface IState
{
    public bool TryAddPlayer(Game game, Player player);
    
    public void Start(Game game);
    
    public void PlayerCallsDirtyLaundry(Game game, Player player);
    
    public void PlayerCallsWhiteLaundry(Game game, Player player);
    
    public void PlayerTurnsLaundry(Game game, Player player, Player victim);
    
    public void BlockLaundryTurnCallsAndWaitForLaundryCalls(Game game);
    
    public void BlockLaundryTurnCallsAndStartRound(Game game);
    
    public void BlockLaundryCalls(Game game);
    
    public void PlayerKnocks(Game game, Player player);
    
    public void PlayerChecks(Game game, Player player);
    
    public void PlayerFolds(Game game, Player player);
    
    public void PlayerPlaysCard(Game game, Player player, Card card);
    
    public Player GetWinner(Game game);
    
    public void StartNewSet(Game game);
}