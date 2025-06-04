using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpeakerProfile", menuName = "Dialogue/Speaker Profile")]
public class SpeakerProfile : ScriptableObject
{
    public string speakerName;
    public Color speechColor;
}
