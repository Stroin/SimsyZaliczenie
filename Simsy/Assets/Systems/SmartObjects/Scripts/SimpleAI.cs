using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseNavigation))]
public class SimpleAI : CommonAIBase
{   
    [SerializeField] protected float PickInteractionInterval = 2f;

    protected float TimeUntilNextInteractionPicked = -1f;

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
                PickRandomInteraction();
            }    
        }
    }
    void PickRandomInteraction()
    {
        // bierze randomowy obiekt
        int objectIndex = Random.Range(0, SmartObjectManager.Instance.RegisteredObjects.Count);
        var selectedObject = SmartObjectManager.Instance.RegisteredObjects[objectIndex];

        // losowa interakcja
        int interactionIndex = Random.Range(0, selectedObject.Interactions.Count);
        var selectedInteraction = selectedObject.Interactions[interactionIndex];

        // Czy może wykonywać interakcje?
        if (selectedInteraction.CanPerform())
        {
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
}
