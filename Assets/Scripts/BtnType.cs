using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class BtnType : MonoBehaviour
{
    public BTNType currentType;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public TMP_Text soundBtnText; // 버튼 안 텍스트 연결할 것
    public GameObject introPanel;
    bool isSound = true;


    public void OnBtnClick()
    {
        switch(currentType)
        {
            case BTNType.New:
                Debug.Log("새게임");
                SceneManager.LoadScene("IntroScene");
                break;
            case BTNType.Continue:
                Debug.Log("이어하기");
                SceneManager.LoadScene("Continue");
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:
                isSound = !isSound;
                if (isSound)
                {
                    Debug.Log("Sound On");
                    soundBtnText.text = "Sound On";
                    AudioListener.volume = 1f;
                }
                else
                {
                    Debug.Log("Sound Off");
                    soundBtnText.text = "Sound Off";
                    AudioListener.volume = 0f;
                }
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case BTNType.Quit:
                Debug.Log("앱종료");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
