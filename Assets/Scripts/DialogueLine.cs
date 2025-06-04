using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public SpeakerProfile speaker;
    [TextArea(2, 5)]
    public string sentence;
}