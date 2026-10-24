using UnityEngine;

public class Coin : LaneHazard
{
    [SerializeField, Min(1)] private int value = 1;
    public int Value => value;
    public override bool DamagesPlayer => false;
}
