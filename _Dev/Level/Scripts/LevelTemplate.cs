using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = "Level Template", menuName =  "SO/Level Template")]
public class LevelTemplate : ScriptableObject
{
    public TemplateChunk[] chunks;
}

public enum ChunkType
{
    Simple,
    Ramp,
    Bridge,
    Overpass,
    Climb,
    Descent,
    Wave,
    Booster,
    Narrow
}

public enum ObstacleType
{
    None = -1,
    Weak = 0,
    Medium,
    Strong,
    Glass,
    Crates,
    Drone
}

public enum PositionEnum
{
    FrontLeft,
    FrontMiddle,
    FrontRight,
    CenterLeft,
    CenterMiddle,
    CenterRight,
    BackLeft,
    BackMiddle,
    BackRight
}
[Serializable]
public class TemplateChunk
{
    public ChunkType ChunkType;
    public Line[] Lines;
    [Serializable]
    public struct Line
    {
        public ObstacleType ObstacleType;
        public PositionEnum Position;
    }
}
