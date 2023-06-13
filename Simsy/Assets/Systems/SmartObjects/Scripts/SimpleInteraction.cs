using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleInteraction : BaseInteraction
{
    protected class PerformerInfo
    {
        public CommonAIBase PerformingAI;
        public float ElapsedTime;
        public UnityAction<BaseInteraction> OnCopleted;
    }

    [SerializeField] protected int MaxSimulataneousUsers = 1;

    protected int NumCurrentUsers = 0;
    protected List<PerformerInfo> CurrentPerformers = new List<PerformerInfo> ();

    public override bool CanPerform()
    {
        return NumCurrentUsers < MaxSimulataneousUsers;
    }

    public override void LockInteraction()
    {
        ++NumCurrentUsers;

        if (NumCurrentUsers > MaxSimulataneousUsers)
            Debug.LogError($"Too many users have locked this interaction {_DisplayName}");
    }

    public override void Perform(CommonAIBase performer, UnityAction<BaseInteraction> onCompleted = null)
    {
        if(NumCurrentUsers <= 0)
        {
            Debug.LogError($"Trying to perform an action when there are no users");
            return;
        }
        
        // sprawdza typ interakcji
        if (InteractionType == EInteractionType.Instantaneous)
        {
            if (StatChanges.Length > 0)
                ApllyStatChanges(performer, 1f);

           onCompleted.Invoke(this);
        }
        else if (InteractionType == EInteractionType.OverTime)
        {
            CurrentPerformers.Add(new PerformerInfo() { PerformingAI = performer,
                                                        ElapsedTime = 0, 
                                                        OnCopleted = onCompleted });
        }
    }

    public override void UnlockInteraction()
    {
        if (NumCurrentUsers <= 0)
            Debug.LogError($"Trying to unlock an already unlocked interaction");

        --NumCurrentUsers;
    }

    protected virtual void Update() 
    {
        // aktualizuje wszelkie bieżące wyniki
        for(int index = CurrentPerformers.Count -1; index >= 0; index--)
        {
            PerformerInfo performer = CurrentPerformers[index];
        
            float previusElapsedTime = performer.ElapsedTime;
            performer.ElapsedTime = Mathf.Min(performer.ElapsedTime + Time.deltaTime, _Duration);

            if (StatChanges.Length > 0)
                ApllyStatChanges(performer.PerformingAI,
                                (performer.ElapsedTime - previusElapsedTime) / _Duration);

            // Zakończona interakcja?
            if (performer.ElapsedTime >= _Duration)
            {
                performer.OnCopleted.Invoke(this);
                CurrentPerformers.RemoveAt(index);
            }
        }
    }
}
