using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {
    [SerializeField]
    private Button NextPageButton;
    [SerializeField]
    private Button PreviousPageButton;
    [SerializeField]
    private Text PageNumberText;
    [SerializeField]
    private Button[] LevelSelectionButtons;

    [SerializeField]
    private int NumberOfLevels;

    private int CurrentPage = 1;
    private int NumberOfPages = 1;

    private void Start()
    {
        
    }
    
}
