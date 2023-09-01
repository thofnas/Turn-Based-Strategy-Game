using System;

public class TurnSystem : Singleton<TurnSystem>
{
    public event EventHandler OnTurnChanged;
    
    private int _turnNumber = 1;

    public void NextTurn()
    {
        _turnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber() => _turnNumber;
}
