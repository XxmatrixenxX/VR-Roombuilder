using UnityEngine;

public class WallPart : MonoBehaviour
{
    public WallScript wallpart;

    public Builded wallBuilded = Builded.Free;

    void Start()
    {
        wallpart.StateChanged += ChangeBuilded;
    }

    public enum Builded
    {
        Wall,
        Window,
        Door,
        Free
    }

    public void ChangeBuilded()
    {
        switch (wallpart.state)
        {
            case WallScript.State.NotVisible:
                wallBuilded = Builded.Free;
                break;
            case WallScript.State.Preview:
                wallBuilded = Builded.Free;
                break;
            case WallScript.State.PlacedWall:
                wallBuilded = Builded.Wall;
                break;
            case WallScript.State.PlacedDoor:
                wallBuilded = Builded.Door;
                break;
            case WallScript.State.PlacedWindow:
                wallBuilded = Builded.Window;
                break;
        }
    }
}