using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Personality
{
    // Define personality traits
    public int Kindness { get; private set; }
    public int Courage { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }

    // Constructor to initialize stats
    public Personality(int kindness, int courage, int wisdom, int charisma)
    {
        Kindness = kindness;
        Courage = courage;
        Wisdom = wisdom;
        Charisma = charisma;
    }

    // Method to adjust a stat by a given amount
    public void AdjustStat(string statName, int amount)
    {
        switch (statName.ToLower())
        {
            case "kindness":
                Kindness += amount;
                break;
            case "courage":
                Courage += amount;
                break;
            case "wisdom":
                Wisdom += amount;
                break;
            case "charisma":
                Charisma += amount;
                break;
            default:
                UnityEngine.Debug.LogError("Invalid stat name: " + statName); // Use UnityEngine.Debug
                break;
        }
    }

    // Method to print the overall stats
    public void PrintStats()
    {
        UnityEngine.Debug.Log("Personality Stats:"); // Use UnityEngine.Debug
        UnityEngine.Debug.Log("Kindness: " + Kindness); // Use UnityEngine.Debug
        UnityEngine.Debug.Log("Courage: " + Courage); // Use UnityEngine.Debug
        UnityEngine.Debug.Log("Wisdom: " + Wisdom); // Use UnityEngine.Debug
        UnityEngine.Debug.Log("Charisma: " + Charisma); // Use UnityEngine.Debug
    }
}
