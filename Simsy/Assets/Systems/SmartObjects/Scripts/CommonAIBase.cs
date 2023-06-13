using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Estat
{
    Energy,
    Fun
}

public class CommonAIBase : MonoBehaviour

{
     [Header ("Fun")]
    [SerializeField] float InitialFunLevel = 0.5f;
    [SerializeField] float BaseFunDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider FunDispaly;

    [Header("Energy")]
    [SerializeField] float InitialEnergyLevel = 0.5f;
    [SerializeField] float BaseEnergyDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider EnergyDispaly;

    protected BaseNavigation Navigation;

    protected BaseInteraction CurrentInteraction = null;
    protected bool StartedPerforming = false;

    public float CurrentFun{ get; protected set; }
    public float CurrentEnergy {get; protected set; }

    protected virtual void Awake()
    {
        FunDispaly.value = CurrentFun = InitialFunLevel;
        EnergyDispaly.value = CurrentEnergy = InitialEnergyLevel;

        Navigation = GetComponent<BaseNavigation>(); 
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CurrentInteraction != null)
        {
            if (Navigation.IsAtDestination && !StartedPerforming)
            {
                StartedPerforming = true;
                CurrentInteraction.Perform(this, OnInteractionFinished);
            }    
        }
    

        CurrentFun = Mathf.Clamp01(CurrentFun - BaseFunDecayRate * Time.deltaTime);
        FunDispaly.value = CurrentFun;
        
        CurrentEnergy = Mathf.Clamp01(CurrentEnergy - BaseEnergyDecayRate * Time.deltaTime);
        EnergyDispaly.value = CurrentEnergy;

    }

    protected virtual void OnInteractionFinished(BaseInteraction interaction)
    {
        interaction.UnlockInteraction();
        CurrentInteraction = null;
        Debug.Log($"Finished {interaction.DisplayName}");
    }

    public void UpdateIndividualStat(Estat target,float amount)
    {
        
        switch(target)
        {
            case Estat.Energy: CurrentEnergy += amount; break;
            case Estat.Fun: CurrentFun += amount; break;
        }
    }
}
