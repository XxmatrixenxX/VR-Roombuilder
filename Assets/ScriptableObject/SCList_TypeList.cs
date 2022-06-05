using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjectList/TypeList")]
public class SCList_TypeList : ScriptableObject
{
    public SC_For_Menu[] menuItemList;

    public string ListTypeName;
}
   