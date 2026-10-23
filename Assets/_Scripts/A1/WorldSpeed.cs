using UnityEngine;

public class WorldSpeed
{
    public const float StartSpeed = 3f;
    public static float Current = StartSpeed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        Current = StartSpeed;
    }
}
