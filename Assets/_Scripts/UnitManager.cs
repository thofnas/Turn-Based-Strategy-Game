using System;
using System.Collections.Generic;

public class UnitManager : Singleton<UnitManager>
{
    private readonly List<Unit> _units = new();
    private readonly List<Unit> _friendlyUnits = new();
    private readonly List<Unit> _enemyUnits = new();

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDied += Unit_OnAnyUnitDied;
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDied -= Unit_OnAnyUnitDied;
    }
    
    public List<Unit> GetUnitList() => _units;
    
    public List<Unit> GetFriendlyUnitList() => _friendlyUnits;
    
    public List<Unit> GetEnemyUnitList() => _enemyUnits;

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        var unit = sender as Unit;

        if (unit != null && unit.IsEnemy())
            _enemyUnits.Add(unit);
        else
            _friendlyUnits.Add(unit);
        
        _units.Add(unit);
    }
    
    private void Unit_OnAnyUnitDied(object sender, EventArgs e)
    {
        var unit = sender as Unit;

        if (unit != null && unit.IsEnemy())
            _enemyUnits.Remove(unit);
        else
            _friendlyUnits.Remove(unit);
        
        _units.Remove(unit);
    }
}
