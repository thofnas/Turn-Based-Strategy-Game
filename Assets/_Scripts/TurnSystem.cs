using System;

public class TurnSystem : Singleton<TurnSystem>
{
    public event EventHandler OnTurnChanged;
    
    private int _turnNumber = 1;
    private bool _isPlayerTurn = true;

    public void NextTurn()
    {
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;
        
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber() => _turnNumber;

    public bool IsPlayerTurn() => _isPlayerTurn;
}
