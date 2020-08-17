using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public LevelDetails levelDetails;
    [SerializeField] private Text levelName;

    private void Awake()
    {
        levelName.text = levelDetails.levelName;
    }
}
