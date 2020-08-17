using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class LevelDetails : ScriptableObject
{
    // Details
    public string levelName;
    public string status;
    
    // Requirements
    public int sanityRequirement;
    
    // Rewards
    public int maxCoin;
    public int starsTaken;
    
    // Best time
    public int bestSeconds;
    public int bestMinutes;
    public int bestHours;
}
