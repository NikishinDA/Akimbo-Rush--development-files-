using System.Collections.Generic;
using UnityEngine;

public class GameEventsHandler
{
    public static readonly GameOverEvent GameOverEvent = new GameOverEvent();
    public static readonly GameStartEvent GameStartEvent = new GameStartEvent();
    public static readonly GamePauseEvent GamePauseEvent = new GamePauseEvent();
    public static readonly CoinPickUpEvent CoinPickUpEvent = new CoinPickUpEvent();
    public static readonly ShowWinPopUp ShowWinPopUp = new ShowWinPopUp();
    public static readonly GameInitializeEvent GameInitializeEvent = new GameInitializeEvent();
    public static readonly PlayerBoostEvent PlayerBoostEvent = new PlayerBoostEvent();
    public static readonly MinigameStartEvent MinigameStartEvent = new MinigameStartEvent();
    public static readonly MinigamePlayerInPositionEvent MinigamePlayerInPositionEvent = new MinigamePlayerInPositionEvent();
    public static readonly EnemyKilledEvent EnemyKilledEvent = new EnemyKilledEvent();
    public static readonly GameProgressEvent GameProgressEvent = new GameProgressEvent();
    public static readonly DebugEvent DebugEvent = new DebugEvent();
    public static readonly PlayerTakeDamageEvent PlayerTakeDamageEvent = new PlayerTakeDamageEvent();
    public static readonly PlayerInstantDeathEvent PlayerInstantDeathEvent = new PlayerInstantDeathEvent();
    public static readonly RampJumpEvent RampJumpEvent = new RampJumpEvent();
    public static readonly PlayerDeadEvent PlayerDeadEvent= new PlayerDeadEvent();
    public static readonly TargetDestroyedEvent TargetDestroyedEvent = new TargetDestroyedEvent();
    public static readonly TransportUnlockEvent TransportUnlockEvent = new TransportUnlockEvent();
    public static readonly MinigamePlayerShotEvent MinigamePlayerShotEvent = new MinigamePlayerShotEvent();
}

public class GameEvent
{
    
}

public class RampJumpEvent : GameEvent
{
    
}

public class GamePauseEvent : GameEvent
{
    public bool SetPause;
}

public class GameOverEvent : GameEvent
{
    public bool IsWin;
    public Vector3 EndPos;
    public Vector3 SecondEndPos;
}

public class PlayerTakeDamageEvent : GameEvent
{
    
}
public class GameProgressEvent : GameEvent { }

public class GameStartEvent : GameEvent
{
    public int LevelSetLength;
    public float SetSpeedZ;
    public float SetSpeedX;
    public float SetSpeedZMax;
}

public class PlayerBoostEvent : GameEvent
{
    
}

public class PlayerInstantDeathEvent : GameEvent
{
    public int ShotType;
}
public class DebugEvent : GameEvent
{
    public float Sense;
    public float Speed;
    public float Boost;
    public float Angle;
    public float Rate;
}

public class CoinPickUpEvent : GameEvent { }
public class WallCrashEvent : GameEvent { }

public class EnemyKilledEvent : GameEvent
{
    public ObstacleType Type;
}

public class GameInitializeEvent : GameEvent
{
    public int LevelLength;
}
public class PlayerDeadEvent : GameEvent
{
}
public class ShowWinPopUp : GameEvent
{
}

public class MinigameStartEvent : GameEvent
{
    
}
public class MinigamePlayerInPositionEvent : GameEvent
{
    
}

public class TargetDestroyedEvent : GameEvent
{
    public int Multiplier;
}

public class TransportUnlockEvent : GameEvent
{
    
}

public class MinigamePlayerShotEvent : GameEvent
{
    
}

public class CameraInPositionEvent : GameEvent
{
    
}

