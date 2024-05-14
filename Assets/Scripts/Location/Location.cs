using DataStructures.RandomSelector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Location : MonoBehaviour
{
    // Events
    public EncounterEvent EncounterStarted = new EncounterEvent();
    public EncounterEvent BossEncounterStarted = new EncounterEvent();
    public EncounterEvent EncounterEnded = new EncounterEvent();
    public EncounterEvent BossEncounterEnded = new EncounterEvent();
    public EncounterStageEvent EncounterStageStarted = new EncounterStageEvent();
    public UnityEvent ProgressBarWasFilled = new UnityEvent();
    public StatTestEvent StatTestFailed = new StatTestEvent();
    public StatTestEvent StatTestSuccessful = new StatTestEvent();
    public delegate WeightedEncounter EncounterEntryDelegate(WeightedEncounter we);
    public EncounterEntryDelegate ModifyEncounterEntries;
    public delegate int IntDelegate(int x);
    public IntDelegate PreUseItemLevel;
    // Weighted Random Selector
    DynamicRandomSelector<WeightedEncounter> selector = new DynamicRandomSelector<WeightedEncounter>();
    //Assign in inspector
    [SerializeField]
    InventorySlot golemSlot;
    [SerializeField]
    public LocationData location;
    [SerializeField]
    Slider progressBar;
    [SerializeField]
    Toggle continouslyRunEncounters;
    [SerializeField]
    GameObject itemTray;
    [SerializeField]
    TextMeshProUGUI EncounterTitle;
    [SerializeField]
    TextMeshProUGUI DifficultyLabel;
    [SerializeField]
    TextMeshProUGUI TestsLabel;
    [SerializeField]
    Image EncounterImage;
    [SerializeField]
    GameObject bossIndicator;
    [SerializeField]
    LocationBalloonInfo locBalloon;
    //Private Variables
    GameObject golemObject;
    GolemInfo golemInfo;
    List<GameObject> generatedItems = new List<GameObject>();
    bool isBossEncounter = false;
    //Readonly public variables
    [ReadOnly]
    public float lootDestroyPercentage = 1f;
    [ReadOnly]
    public Encounter currentEncounter;
    [ReadOnly]
    public EncounterStage currentEncounterStage;
    [ReadOnly]
    public float baseActionLength;
    public float actionLengthModifier = 1;
    float TrueActionLength { get { return baseActionLength * actionLengthModifier; } }
    [ReadOnly]
    public int EncounterProgress;
    [ReadOnly]
    public int LocationProgress = 0;
    [ReadOnly]
    bool encounterIsRunning = false;
    // Start is called before the first frame update
    void Awake()
    {
        golemSlot.gameObjectDropped.AddListener(GolemAdded);
        golemSlot.gameObjectLoaded.AddListener(GolemAdded);
        golemSlot.gameObjectRemoved.AddListener(GolemRemoved);
    }

    [Button]
    public void RollEncounter()
    {
        if (golemObject == null)
            return;
        LockGolemIn();
        selector = new DynamicRandomSelector<WeightedEncounter>();
        List<WeightedEncounter> list2SelectFrom = new List<WeightedEncounter>();
        if (location.locationHasProgressEncounters && location.MaxProgress <= LocationProgress)
        {
            Debug.Log("boss tim");
            LocationProgress = 0;
            list2SelectFrom = location.bossEncounters;
            isBossEncounter = true;
        }
        else
        {
            list2SelectFrom = location.encounters;
            isBossEncounter = false;
        }
        foreach (WeightedEncounter we in list2SelectFrom)
        {
            WeightedEncounter encounter = new WeightedEncounter()
            {
                encounter = we.encounter,
                weight = we.weight
            };
            if(ModifyEncounterEntries != null)
                encounter = ModifyEncounterEntries(encounter);
            selector.Add(encounter, encounter.weight);
        }
        selector.Build();
        StartEncounter(selector.SelectRandomItem().encounter);
        
    }
    public void StartEncounter(Encounter encounter)
    {
        currentEncounter = encounter;
        if (isBossEncounter && BossEncounterStarted != null)
            BossEncounterStarted.Invoke(currentEncounter);
        if (EncounterStarted != null)
            EncounterStarted.Invoke(currentEncounter);
        
        SetupEncounterStage(0);
    }
    public void SetupEncounterStage(int index)
    {
        currentEncounterStage = currentEncounter.EncounterStages[index];
        EncounterTitle.text = currentEncounter.EncounterTitle + ": " + currentEncounterStage.StageTitle;
        if(currentEncounterStage.StageImage != null)
            EncounterImage.sprite = currentEncounterStage.StageImage;
        int currentdifficulty = 0;
        TestsLabel.text = "Likely to Test: ";
        foreach (StatTest st in currentEncounterStage.statTests)
        {
            if (st.difficultyThreshold > currentdifficulty)
                currentdifficulty = st.difficultyThreshold;
            TestsLabel.text += (st.statToTest.ToString() + ",");
        }
        DifficultyLabel.text = "Difficulty: ";
        DifficultyLabel.text += currentdifficulty.ToString();
        EncounterProgress = 0;
        if (EncounterStageStarted != null)
            EncounterStageStarted.Invoke(currentEncounterStage);
        baseActionLength = currentEncounterStage.BaseActionLength;
        progressBar.value = 0;
        encounterIsRunning = true;
    }
    private void Update()
    {
        if (!encounterIsRunning)
            return;
        if (golemInfo.isDead)
        {
            lootDestroyPercentage = 1;
            GolemDied();
        }
        if (progressBar.value == 1)
            ProgressBarFilled();
        progressBar.value +=  Time.deltaTime/TrueActionLength;

    }
    private void GolemDied()
    {
        EndEncounter(false);
        golemInfo.Die();
        golemSlot.currentlyHolding = null;
        golemInfo = null;
        foreach (Transform t in itemTray.transform)
            if(Random.Range(0,1f) <= lootDestroyPercentage)
                Destroy(t.gameObject);
        foreach (GameObject item in generatedItems)
            if(item != null)
                InventoryManager.Instance.AddToRecentInventory(item.gameObject);
        generatedItems = new List<GameObject>();
    }
    private void EndEncounter(bool wasSuccessful)
    {
        EncounterTitle.text = "No Current Encounter";
        EncounterImage.sprite = null;
        encounterIsRunning = false;
        EncounterProgress = 0;
        progressBar.value = 0;
        if (isBossEncounter)
        {
            bossIndicator.SetActive(false);
            if (BossEncounterEnded != null)
                BossEncounterEnded.Invoke(currentEncounter);
            if (wasSuccessful)
                locBalloon.CompleteLocation(currentEncounterStage);
        }
        if (LocationProgress >= location.MaxProgress)
            bossIndicator.SetActive(true);
        if (EncounterEnded != null)
            EncounterEnded.Invoke(currentEncounter);
        wasSuccessful = false;
        currentEncounter = null;
        isBossEncounter = false;
        currentEncounterStage = null;
        UnlockGolem();
    }
    public void Abort()
    {
        if (golemInfo == null)
            return;
        lootDestroyPercentage = 0.5f;
        golemInfo.EncounterWasAborted(this);
        GolemDied();
    }
    void ProgressBarFilled()
    {
        bool hasFailed = false;
        progressBar.value = 0;
        if (ProgressBarWasFilled != null)
            ProgressBarWasFilled.Invoke();
        foreach(StatTest st in currentEncounterStage.statTests)
        {
            int roll = Random.Range(1, 11);
            roll += golemInfo.GetStatValue(st.statToTest);
            if (roll < st.difficultyThreshold)
            {
                if (StatTestFailed != null)
                    StatTestFailed.Invoke(st);
                hasFailed = true;
            }
            else
            {
                if (StatTestSuccessful != null)
                    StatTestSuccessful.Invoke(st);
            }
                
        }
        if (hasFailed)
        {
            EncounterResult r = currentEncounterStage.FailureResult;
            EvaluateEncounterResult(r);
        }
        else
        {
            EncounterProgress++;
            if(EncounterProgress >= currentEncounterStage.StageHP)
            {
                EncounterProgress = 0;
                EncounterResult r = currentEncounterStage.SuccessResult;
                EvaluateEncounterResult(r);
                if (currentEncounter.EncounterStages.IndexOf(currentEncounterStage) == currentEncounter.EncounterStages.Count - 1)
                {
                    EndEncounter(true);
                    foreach (GameObject item in generatedItems)
                        InventoryManager.Instance.AddToRecentInventory(item.gameObject);
                    generatedItems = new List<GameObject>();
                    if (continouslyRunEncounters.isOn)
                        RollEncounter();
                }
                else
                {
                    SetupEncounterStage(currentEncounter.EncounterStages.IndexOf(currentEncounterStage) + 1);
                }
            }
        }
        if (golemInfo != null)
            golemInfo.GetComponent<TooltipCallerUi>().BuildTooltip();
    }
    public void EvaluateEncounterResult(EncounterResult r)
    {
        LocationProgress += r.LocationProgress;

        if (r.lootTable != null)
        {
            int rolls = Random.Range(r.minRolls, r.maxRolls);
            int newIlevel = r.baseItemLevel;
            if (PreUseItemLevel != null)
                newIlevel = PreUseItemLevel.Invoke(newIlevel);
            ItemRoller.Instance.RollItems(r.lootTable, golemObject, this, rolls, newIlevel, itemTray.transform);
            foreach (Transform t in itemTray.transform)
                if (!generatedItems.Contains(t.gameObject))
                    generatedItems.Add(t.gameObject);
        }
        golemInfo.TakeDamage(r.damageNumber, r.damageType);
    }
    public void ShowBossIndicator()
    {
        bossIndicator.SetActive(true);
    }
    void GolemAdded(GameObject go)
    {
        golemObject = go;
        go.GetComponent<InventoryContextMenu>().contextMenuEnabled = false;
        golemInfo = go.GetComponent<GolemInfo>();
        golemInfo.currentLocation = this;
        golemInfo.SetupLocationDelegates(this);
    }
    void GolemRemoved(GameObject go)
    {
        golemInfo.RemoveAllLocatonListeners(this);
        go.GetComponent<InventoryContextMenu>().contextMenuEnabled = true;
        golemInfo.currentLocation = null;
        golemObject = null;
        golemInfo = null;
        ModifyEncounterEntries = null;
        PreUseItemLevel = null;
    }
    [Button]
    void LockGolemIn()
    {
        golemObject.GetComponent<ItemDrag>().canDrag = false;
        golemSlot.isDroppable = false;
    }
    [Button]
    void UnlockGolem()
    {
        if(golemObject != null)
            golemObject.GetComponent<ItemDrag>().canDrag = true;
        golemSlot.isDroppable = true;
    }
}



public class EncounterEvent : UnityEvent<Encounter>
{

}
public class EncounterStageEvent : UnityEvent<EncounterStage>
{

}
public class StatEvent : UnityEvent<GlobalDefinitions.Stats>
{

}
public class StatTestEvent : UnityEvent<StatTest>
{

}