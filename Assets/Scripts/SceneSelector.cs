using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour
{
    public Button scene1Button;
    public Button scene2Button;
    public Button scene3Button;
    public Button titleButton;

    void Start()
    {
        // Scene1을 거쳤는지 체크
        bool unlocked = PlayerPrefs.GetInt("Scene1Visited", 0) == 1;

        scene1Button.interactable = unlocked;
        scene2Button.interactable = unlocked;
        scene3Button.interactable = unlocked;
        titleButton.interactable = true;

        scene1Button.onClick.AddListener(() => LoadScene("Scene1"));
        scene2Button.onClick.AddListener(() => LoadScene("Scene2"));
        scene3Button.onClick.AddListener(() => LoadScene("Scene3"));
        titleButton.onClick.AddListener(() => LoadScene("Scene0"));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

