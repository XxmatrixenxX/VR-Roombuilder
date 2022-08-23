using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjectList/RoomList")]
public class SC_For_RoomList : ScriptableObject
{
    public List<SC_For_Menu> roomObject;

    public string ListTypeName;
}
