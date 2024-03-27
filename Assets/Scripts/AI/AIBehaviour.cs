using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    [SerializeField] protected float weightMultiplier = 1;
    [SerializeField] protected float timePassed = 0;

    public float TimePassed
    {
        get => timePassed;
        set => timePassed = value;
    }

    public float WeightMultiplier => weightMultiplier;

    public abstract float GetWeight();
    public abstract void Execute();
}
