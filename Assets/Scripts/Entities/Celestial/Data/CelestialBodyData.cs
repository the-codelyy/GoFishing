using UnityEngine;

public enum CelestialBodyType
{
    Star,
    Planet,
    Unknown,
}

public abstract class CelestialBodyData : ScriptableObject
{
    public string Name;
    public CelestialBodyType Type;
}
