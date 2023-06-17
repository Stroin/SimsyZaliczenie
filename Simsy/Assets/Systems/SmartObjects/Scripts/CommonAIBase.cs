using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStat
{
    Energy,
    Fun,
    Hungry
}

[RequireComponent(typeof(BaseNavigation))]
public class CommonAIBase : MonoBehaviour
{
    [Header("Fun")]
    [SerializeField] float InitialFunLevel = 0.9f;
    [SerializeField] float BaseFunDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider FunDisplay;

    [Header("Energy")]
    [SerializeField] float InitialEnergyLevel = 0.9f;
    [SerializeField] float BaseEnergyDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider EnergyDisplay;

    [Header("Hungry")] 
    [SerializeField] float InitialHungryLevel = 0.9f;
    [SerializeField] float BaseHungryDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider HungryDisplay;

    protected BaseNavigation Navigation;

    protected BaseInteraction CurrentInteraction = null;
    protected bool StartedPerforming = false;

    public float CurrentFun { get; protected set; }
    public float CurrentEnergy { get; protected set; }
    public float CurrentHungry { get; protected set; }
  
  protected virtual void Awake()
    {
        FunDisplay.value = CurrentFun = InitialFunLevel;
        EnergyDisplay.value = CurrentEnergy = InitialEnergyLevel;
        HungryDisplay.value = CurrentHungry = InitialHungryLevel;

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
        FunDisplay.value = CurrentFun;

        CurrentEnergy = Mathf.Clamp01(CurrentEnergy - BaseEnergyDecayRate * Time.deltaTime);
        EnergyDisplay.value = CurrentEnergy;

        CurrentHungry = Mathf.Clamp01(CurrentHungry - BaseHungryDecayRate * Time.deltaTime);
        HungryDisplay.value = CurrentHungry;
    }

    protected virtual void OnInteractionFinished(BaseInteraction interaction)
    {
        interaction.UnlockInteraction();
        CurrentInteraction = null;
        Debug.Log($"Finished {interaction.DisplayName}");
    }

    public void UpdateIndividualStat(EStat target, float amount)
    {
        switch(target)
        {
            case EStat.Energy: CurrentEnergy += amount; break;
            case EStat.Fun:    CurrentFun += amount; break;
            case EStat.Hungry: CurrentHungry += amount; break;
        }
    }    
}