using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum EInteractionType
{
    Instantaneous = 0,
    OverTime = 1
}

[System.Serializable]
public class InteractionStatChange
{
    public Estat Target;
    public float Value;
}

public abstract class BaseInteraction : MonoBehaviour
{
    [SerializeField] protected string _DisplayName;
    [SerializeField] protected EInteractionType _InteractionType = EInteractionType.Instantaneous;
    [SerializeField] protected float _Duration = 0f;
    [SerializeField, FormerlySerializedAs("StatChanges")] protected InteractionStatChange[] _StatChanges;

    public string DisplayName => _DisplayName;
    public EInteractionType InteractionType => _InteractionType;
    public float Duration => _Duration;
    public InteractionStatChange[] StatChanges => _StatChanges;
    public abstract bool CanPerform();
    public abstract void LockInteraction();
    public abstract void Perform(CommonAIBase performer, UnityAction<BaseInteraction> onCompleted = null);
    public abstract void UnlockInteraction();
    public void ApllyStatChanges(CommonAIBase performer, float proportion)
    {
        foreach(var StatChange in StatChanges)
        {
            performer.UpdateIndividualStat(StatChange.Target, StatChange.Value * proportion);
        }
    }
}
