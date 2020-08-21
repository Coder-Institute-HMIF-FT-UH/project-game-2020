using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class LevelDetails : ScriptableObject
{
    // Details
    public string levelName;
    public string sceneName;
    
    // Requirements
    public int sanityRequirement;
    [Range(0, 3)] public int starRequirement;
    
    // Rewards
    public int maxCoin;
}
