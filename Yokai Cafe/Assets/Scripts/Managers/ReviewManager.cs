using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviewManager : MonoBehaviour
{
    public static ReviewManager Instance;
    public GameObject reviewScreen;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI earningsText;
    public Button nextDayButton;
    public GameObject player; // assign the Player GameObject in the Inspector
    private MonoBehaviour[] playerScripts;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        reviewScreen.SetActive(false);
        nextDayButton.onClick.AddListener(OnNextDayClicked);

        playerScripts = player.GetComponents<MonoBehaviour>();
        // filter only movement/camera scripts if there are extra?

    }

    public void Show(int day, int earnings)
    {
        reviewScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // pause the game
        // Disable player scripts
        foreach (var script in playerScripts)
        {
            script.enabled = false;
        }

        dayText.text = "Day " + day;
        earningsText.text = "Earnings: " + earnings;
    }

    public void OnNextDayClicked()
    {
        reviewScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f; // resume the game
        // Re-enable player scripts
        foreach (var script in playerScripts)
        {
            script.enabled = true;
        }

        DayManager.Instance.StartNextDay();
    }
}
