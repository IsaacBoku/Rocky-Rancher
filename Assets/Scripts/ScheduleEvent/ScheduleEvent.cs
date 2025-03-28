using UnityEngine;

[System.Serializable]
public struct ScheduleEvent
{
    public string name;
    [Header("Conditions")]
    public GameTimeStap time;

    public GameTimeStap.DayOfTheWeek dayOfTheWeek;

    public int priority;

    public bool factorDate;
    public bool ignoreDayOfTheWeek;

    [Header("Position")]
    public Trasition_Scenes.Locations locations;
    public Vector3 coord;
    public Vector3 facing;




}
