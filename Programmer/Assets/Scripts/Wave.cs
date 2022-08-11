using System;

[Serializable]
public class Wave
{
    public int Count;
    public float NextObstacleDelay = 0.5f;
    public float NextWaveDelay = 5f;
    public Obstacle Obstactle;
}
