using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [SerializeField] GameTimeStap timestamp;
    public float timeScale = 1.0f;

    public Transform sunTransform;
    Vector3 sunAngle;
    private float indoorAngle = 40;
    List<ITimeTracker> listeners = new List<ITimeTracker>();
    private void Awake()
    {
        if(Instance !=null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        timestamp = new GameTimeStap(0, GameTimeStap.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }
    private void Update()
    {
        sunTransform.rotation = Quaternion.Slerp(sunTransform.rotation, Quaternion.Euler(sunAngle), 1f * Time.deltaTime);
    }
    public void LoadTime(GameTimeStap timeStap)
    {
        this.timestamp = new GameTimeStap(timeStap);
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeScale+Time.deltaTime);
        }
    }
    public void Tick()
    {
        timestamp.UpdateClock();
        UpdateSunMovement();
        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }
    }
    public void SkipTime(GameTimeStap timeToSkipTo)
    {
        int timeToSkipInMinutes = GameTimeStap.TimestampInMinutes(timeToSkipTo);
        int timeNowInMinutes = GameTimeStap.TimestampInMinutes(timestamp);
        int differenceInMinutes = timeToSkipInMinutes - timeNowInMinutes;
        if (differenceInMinutes <= 0) return;
        for (int i = 0; i < differenceInMinutes; i++)
        {
            Tick();
        }
    }
    void UpdateSunMovement()
    {
        if (Trasition_Scenes.instance.CurrentlyIndoor())
        {
            this.sunAngle = new Vector3(indoorAngle, 0, 0);
            return;
        }
        int timeInMinutes = GameTimeStap.HourToMinute(timestamp.hour) + timestamp.minute;
        float sunAngle = .25f * timeInMinutes - 100;
        //sunTransform.eulerAngles = new Vector3(sunAngle, 50, 0);
        //Apply angle to the directional light
        //sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
        this.sunAngle = new Vector3(sunAngle, 2, 0);
    }
    public GameTimeStap GetGameTimesStap()
    {
        return new GameTimeStap(timestamp);
    }
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }
    public void UnRegisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }


}
