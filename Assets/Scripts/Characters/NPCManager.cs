using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour,ITimeTracker
{

    public static NPCManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    List<CharacterData> characters = null;

    List<NPCScheduleData> npcSchedules;

    [SerializeField] List<NPCLocationState> npcLocations;


    public List<CharacterData> Characters()
    {
        if (characters != null) return characters;
        CharacterData[] characterDatabase = Resources.LoadAll<CharacterData>("Characters");
        characters = characterDatabase.ToList();
        return characters;
    }

    private void OnEnable()
    {
        NPCScheduleData[] schedules = Resources.LoadAll<NPCScheduleData>("Schedules");
        npcSchedules = schedules.ToList();
        InitNPCLocations();
    }
    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
        Trasition_Scenes.instance.onLocationLoad.AddListener(RenderNPCs);
    }

    private void InitNPCLocations()
    {
        npcLocations = new List<NPCLocationState>();
        foreach(CharacterData character in Characters())
        {
            npcLocations.Add(new NPCLocationState(character));
        }
    }

    void RenderNPCs()
    {
        foreach(NPCLocationState npc in npcLocations)
        {
            if(npc.location == Trasition_Scenes.instance.currentLocation)
            {
                Instantiate(npc.character.prefab, npc.coord, Quaternion.Euler(npc.facing));
            }
        }
    }
    void SpawnInNpc(CharacterData npc, Trasition_Scenes.Locations comingFrom)
    {
        Transform start = LocationManager.Instance.GetPlayerStartingPosition(comingFrom);
        Instantiate(npc.prefab,start.position,start.rotation);
    }
    public void ClockUpdate(GameTimeStap timestamp)
    {
        UpdateNPCLocations(timestamp);
    }
    public NPCLocationState GetNPCLocation(string name)
    {
        return npcLocations.Find(x => x.character.name == name);
    }

    private void UpdateNPCLocations(GameTimeStap timestamp)
    {
        for (int i = 0; i < npcLocations.Count; i++)
        {
            NPCLocationState npcLocator = npcLocations[i];
            Trasition_Scenes.Locations previousLocation = npcLocator.location;
            NPCScheduleData schedule = npcSchedules.Find(x=> x.character == npcLocator.character);
            if(schedule == null)
            {
                Debug.LogError("No schedule found for " + npcLocator.character.name);
                continue;
            }

            GameTimeStap.DayOfTheWeek dayOfWeek = timestamp.GetDayOfTheWeek();
            List<ScheduleEvent> eventsToConsider = schedule.npcScheduleList.FindAll(x=> x.time.hour <= timestamp.hour && (x.dayOfTheWeek == dayOfWeek || x.ignoreDayOfTheWeek));
            if(eventsToConsider.Count < 1)
            {
                Debug.LogError("None found for " + npcLocator.character.name);
                Debug.LogError(timestamp.hour);
                continue;
            }
            int maxHour = eventsToConsider.Max(x => x.time.hour);
            eventsToConsider.RemoveAll(x => x.time.hour < maxHour);

            ScheduleEvent eventToExecute = eventsToConsider.OrderByDescending(x => x.priority).First();
            Debug.Log(eventToExecute.name);
            npcLocations[i] = new NPCLocationState(schedule.character, eventToExecute.locations, eventToExecute.coord,eventToExecute.facing);

            Trasition_Scenes.Locations newLocation = eventToExecute.locations;

            if(newLocation != previousLocation)
            {
                Debug.Log("New location" + newLocation);
                if(Trasition_Scenes.instance.currentLocation == newLocation)
                {
                    SpawnInNpc(schedule.character, previousLocation);
                }
            }
        }



    }
}
