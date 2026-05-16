using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SystemBodyData
{
    public CelestialBodyData Body;
    public float DistanceFromPrimary;
    public float OrbitSpeed = 1.0f;
    public float Size;
}

[CreateAssetMenu(fileName = "StarSystem", menuName = "Celestial/Star System", order = -1)]
public class StarSystemData : ScriptableObject
{
    public string Name;
    public CelestialBodyData Star;
    public List<SystemBodyData> Satellites;
}
