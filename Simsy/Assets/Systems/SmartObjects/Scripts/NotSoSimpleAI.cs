using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BaseNavigation))]
public class NotSoSimpleAI : CommonAIBase
{ 
   [SerializeField] protected float PickInteractionInterval = 2f;  
   [SerializeField] protected float DefaultInteractionScore = 0f;
   [SerializeField] protected int InteractionPickSize = 5;
   
   protected float TimeUntilNextInteractionPicked = -1f;

    // Start is called before the first frame update
   protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (CurrentInteraction == null)
        {
            TimeUntilNextInteractionPicked -= Time.deltaTime;

            // Czas do podjęcia interakcji
            if (TimeUntilNextInteractionPicked <= 0)
            {
                TimeUntilNextInteractionPicked = PickInteractionInterval;
                PickBestInteraction();
            }    
        }
    }

   
    float ScoreInteraction(BaseInteraction interaction)
    {
        if (interaction.StatChanges.Length ==0)
        {
            return DefaultInteractionScore;
        }

        float score = 0f;
        foreach(var change in interaction.StatChanges)
            score += ScoreChange(change.Target, change.Value);

            return score;
        }

     float ScoreChange(Estat target, float amount)
    {
        float currentValue = 0f;
          switch(target)
        {
            case Estat.Energy: currentValue =CurrentEnergy; break;
            case Estat.Fun: CurrentFun += CurrentFun; break;
        }

        return (1f - currentValue) * amount;
    }

    class ScoredInteraction
    {
        public SmartObject TargerObject;
        public BaseInteraction Interaction;
        public float Score;
    }
     void PickBestInteraction()
    {
        // przechodzi przez wszystkie obiekty
        List<ScoredInteraction> unsortedInteractions = new List<ScoredInteraction>();
        foreach (var smartObject in SmartObjectManager.Instance.RegisteredObjects)
        {
            // przechodzi przez wszystkie interakcje
            foreach (var interaction in smartObject.Interactions)
            {
                if (interaction.CanPerform())
                    continue;

                    float score = ScoreInteraction(interaction);

                    unsortedInteractions.Add(new ScoredInteraction() {  TargerObject = smartObject,
                                                                        Interaction = interaction,
                                                                        Score = score});
            }
        }

        if (unsortedInteractions.Count ==0)
            return;

        //sortuje i wybiera jedną z najlepszy interakcji
        var sortedInteractions = unsortedInteractions.OrderBy(ScoredInteraction => ScoredInteraction.Score).ToList();    
        int maxIndex = Mathf.Min(InteractionPickSize, sortedInteractions.Count);

        var selecctedIndex = Random.Range(0, maxIndex);

        var selectedObject = sortedInteractions[selecctedIndex].TargerObject;
        var selectedInteraction = sortedInteractions[selecctedIndex].Interaction;
       
        CurrentInteraction = selectedInteraction;
            CurrentInteraction.LockInteraction();
            StartedPerforming = false;

            // idzie do celu
            if   (!Navigation.SetDestination(selectedObject.InteractionPoint))
            {
                Debug.LogError($"Could not move to {selectedObject.name}");
                CurrentInteraction = null;                
            }
            else
                Debug.Log($"Going to {CurrentInteraction.DisplayName} at {selectedObject.DisplayName}");
    } 
}
 