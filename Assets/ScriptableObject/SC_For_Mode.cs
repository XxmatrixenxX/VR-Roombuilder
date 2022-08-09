using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ModeItem")]
public class SC_For_Mode : ScriptableObject
{
    public Mode mode;

    public string modeString;
    public string description;

    public enum Mode
    {
        buildingMode,
        chooseBuildingMode,
        topBuildingMode,
        playerMode,
        wallBuildingMode,
        savingMode
    }
}
