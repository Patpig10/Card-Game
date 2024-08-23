using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Personality personality;

    private void Start()
    {
        // Initialize personality with starting values
        personality = new Personality(kindness: 10, courage: 5, wisdom: 8, charisma: 7);

        // Print initial stats
        personality.PrintStats();

        // Adjust some stats as an example
        personality.AdjustStat("Kindness", 2);
        personality.AdjustStat("Courage", -1);

        // Print updated stats
        personality.PrintStats();
    }
}
