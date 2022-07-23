using UnityEngine;

public class FunitureWithPlaceArea : Funiture
{
    [SerializeField] private Transform placeArea;

    public float gettingHightOfFuniture()
    {
        return placeArea.position.y;
    }
}
    
