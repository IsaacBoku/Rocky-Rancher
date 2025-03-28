using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using static Trasition_Scenes;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }

    public List<StartPoint> startPoints;

    private static readonly Dictionary<Locations, List<Locations>> sceneConnections = new Dictionary<Locations, List<Locations>>() {{ Locations.Homes, new List<Locations> {Locations.Farm} },{Locations.Farm,new List<Locations>{Locations.Homes,Locations.Town}},{Locations.Town, new List<Locations>{ Locations.Farm,Locations.Taberna } },{Locations.Taberna,new List<Locations>{Locations.Town}
    } };
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
           Instance = this;
    }
    public Transform GetPlayerStartingPosition(Trasition_Scenes.Locations enteringFrom)
    {
        StartPoint startingPoint = startPoints.Find(x => x.enteringFrom == enteringFrom);
        return startingPoint.playerStart;
    }
    public Transform GetExitPosition(Locations exitingTo)
    {
        Transform startPoint = GetPlayerStartingPosition(exitingTo);
        return startPoint.parent.GetComponentInChildren<LocationEntryPoint>().transform;
    }

    public static Locations GetNextLocation(Locations currentScene,Locations finalDestination)
    {
        Dictionary<Locations,bool> visited = new Dictionary<Locations,bool>();

        Dictionary<Locations,Locations> previousLocation = new Dictionary<Locations,Locations>();

        Queue<Locations> worklist = new Queue<Locations>();

        visited.Add(currentScene, false);
        worklist.Enqueue(currentScene);

        while(worklist.Count != 0)
        {
            Locations scene = worklist.Dequeue();
            if(scene == finalDestination)
            {
                while(previousLocation.ContainsKey(scene)&& previousLocation[scene] != currentScene)
                {
                    scene = previousLocation[scene];
                }
                return scene;
            }
            if(sceneConnections.ContainsKey(scene))
            {
                List<Locations> possibleDestinations = sceneConnections[scene];

                foreach(Locations neighbour in possibleDestinations)
                {
                    if(!visited.ContainsKey(neighbour))
                    {
                        visited.Add(neighbour, false);
                        previousLocation.Add(neighbour, scene);
                        worklist.Enqueue(neighbour);
                    }
                }
            }
        }
        return currentScene;
    }

}
