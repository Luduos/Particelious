using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {
    [SerializeField]
    private Button NextPageButton = null;
    [SerializeField]
    private Button PreviousPageButton = null;
    [SerializeField]
    private Text PageNumberText = null;
    [SerializeField]
    private Button[] LevelSelectionButtons = new Button[10];

    [SerializeField]
    private int NumberOfLevels = 0;

    private int CurrentPage = 1;
    private int NumberOfPages = 1;

    private void Start()
    {
        float fNumberOfPages = (float) NumberOfLevels / LevelSelectionButtons.Length;
        NumberOfPages = Mathf.CeilToInt(fNumberOfPages);

        NextPageButton.onClick.AddListener(NextPage);
        PreviousPageButton.onClick.AddListener(PreviousPage);

        UpdateAndCheckPageInformation();
    }
    
    private void StartLevel(int levelID)
    {
        SceneManager.LoadScene("Level_" + levelID);
        GlobalInfo.instance.CurrentLevel = levelID;
    }

    private void PreviousPage()
    {
        CurrentPage--;
        UpdateAndCheckPageInformation();
    }

    private void NextPage()
    {
        CurrentPage++;
        UpdateAndCheckPageInformation();
    }

    private void UpdateAndCheckPageInformation()
    {
        CheckPageButtonsEnabled();
        ReloadLevelSelectionPage();
        UpdatePageNumberText();
    }

    private void UpdatePageNumberText()
    {
        Text ButtonText = PageNumberText.GetComponentInChildren<Text>();
        ButtonText.text = CurrentPage + "/" + NumberOfPages;
    }

    private void CheckPageButtonsEnabled()
    {
        PreviousPageButton.interactable = CurrentPage > 1;
        NextPageButton.interactable = CurrentPage < NumberOfPages;
    }

    private void ReloadLevelSelectionPage()
    {
        for (int i = 0; i < LevelSelectionButtons.Length; ++i)
        {
            LevelSelectionButtons[i].onClick.RemoveAllListeners();
            int CorrespondingLevelNumber = (i + 1) + (CurrentPage - 1) * 10;

            // set the corresponding Listener function
            LevelSelectionButtons[i].onClick.AddListener(() => { StartLevel(CorrespondingLevelNumber); });
            // set the displayed text
            Text ButtonText = LevelSelectionButtons[i].GetComponentInChildren<Text>();
            ButtonText.text = CorrespondingLevelNumber.ToString();

            // Only set Button active, if there's a corresponding level
            LevelSelectionButtons[i].gameObject.SetActive(CorrespondingLevelNumber <= NumberOfLevels);
        }
    }
}
