using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class LevelDetails : ScriptableObject
{
    // Details
    public string levelName;
    public bool isClear;
    public string sceneName;
    
    // Requirements
    public int sanityRequirement;
    [Range(0, 3)] public int starRequirement;
    
    // Rewards
    public int maxCoin;
    [Range(0, 3)] public int starsTaken;
    
    // Best time
    [Range(0, 59)] public int bestSeconds;
    [Range(0, 59)] public int bestMinutes;
    public int bestHours;
}
