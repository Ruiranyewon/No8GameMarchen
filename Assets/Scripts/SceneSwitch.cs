using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void LoadScene1()
    {
        Debug.Log("LoadScene1 called");
        SceneManager.LoadScene("Scene1");
    }
    public void LoadScene2()
    {
        SceneManager.LoadScene("Scene2");
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene("Scene3");
    }

    public void LoadScene4()
    {
        SceneManager.LoadScene("Scene4");
    }
}