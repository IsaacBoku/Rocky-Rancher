using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocationManager))]
public class TrasitionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Output Coords"))
        {
            GameObject markerParent = GameObject.Find("NPC Markers");
            foreach(Transform point in markerParent.transform)
            {
                Debug.Log(point.gameObject.name +" " + point.position + " " + point.rotation.eulerAngles);
            }

        }
    }
}
