using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject worldEditorCanvas;
    [SerializeField] private GameObject worldLoaderCanvas;
    [SerializeField] private GameObject roomBuilderCanvas;
    [SerializeField] private GameObject roomLoaderCanvas;
    [SerializeField] private GameObject optionsCanvas;

    [SerializeField] private GameObject worldContainer;
    [SerializeField] private GameObject roomContainer;

    [SerializeField] private Scene newRoomScene;
    [SerializeField] private Scene newWorldScene;
    [SerializeField] private Scene loadWorldScene;
    [SerializeField] private Scene loadRoomScene;
    
    
    [SerializeField] private UIMenuItem roomUI;
    [SerializeField] private SC_For_RoomList roomList;
    [SerializeField] private SC_For_RoomList worldList;
    [SerializeField] private LoadingScript loadingScript;

    private void Start()
    {
        if (FindObjectOfType<LoadingScript>() != null)
        {
            loadingScript = FindObjectOfType<LoadingScript>();
            Destroy(loadingScript);
        }

        loadingScript = Instantiate(new GameObject("loadingScript").AddComponent<LoadingScript>());
        loadingScript.AddComponent<DontDestroyOnLoad>();
    }


    public void OpenUIWorldEditor()
    {
        worldEditorCanvas.SetActive(true);
        worldLoaderCanvas.SetActive(false);
        roomBuilderCanvas.SetActive(false);
        roomLoaderCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }
    
    public void OpenUIRoomBuilder()
    {
        worldEditorCanvas.SetActive(false);
        worldLoaderCanvas.SetActive(false);
        roomBuilderCanvas.SetActive(true);
        roomLoaderCanvas.SetActive(false);
        optionsCanvas.SetActive(false);  
    }
    
    public void OpenUIWorldLoader()
    {
        worldEditorCanvas.SetActive(false);
        worldLoaderCanvas.SetActive(true);
        roomBuilderCanvas.SetActive(false);
        roomLoaderCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        FillWorldInContainer();
    }
    
    public void OpenUIRoomLoader()
    {
        worldEditorCanvas.SetActive(false);
        worldLoaderCanvas.SetActive(false);
        roomBuilderCanvas.SetActive(false);
        roomLoaderCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
        FillRoomInContainer();
    }
    
    public void OpenUIOptions()
    {
        worldEditorCanvas.SetActive(false);
        worldLoaderCanvas.SetActive(false);
        roomBuilderCanvas.SetActive(false);
        roomLoaderCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    
    public void CloseButton()
    {
        worldEditorCanvas.SetActive(false);
        worldLoaderCanvas.SetActive(false);
        roomBuilderCanvas.SetActive(false);
        roomLoaderCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadingScene(Scene scene)
    {
        SceneManager.LoadScene(scene.name);
    }
    
    public void FillRoomInContainer()
    {
        foreach (Transform exampleDelete in roomContainer.transform)
        {
            GameObject.Destroy(exampleDelete.gameObject);
        }
        
        foreach (var sc_menu in roomList.roomObject)
        {
            UIMenuItem menu = Instantiate(roomUI, new Vector3(0, 0, 0), Quaternion.identity);

            menu.transform.SetParent(roomContainer.transform, false);
            
            menu.title.text = sc_menu.itemName;
            menu.button.onClick.AddListener(delegate { ClickedRoom(sc_menu.item); });
        }
    }
    
    public void FillWorldInContainer()
    {
        foreach (Transform exampleDelete in worldContainer.transform)
        {
            Destroy(exampleDelete.gameObject);
        }
        
        foreach (var sc_menu in worldList.roomObject)
        {
            UIMenuItem menu = Instantiate(roomUI, new Vector3(0, 0, 0), Quaternion.identity);

            menu.transform.SetParent(worldContainer.transform, false);
            
            menu.title.text = sc_menu.itemName;
            menu.button.onClick.AddListener(delegate { ClickedWorld(sc_menu.item); });
        }
    }

    public void LoadNewRoom()
    {
        SceneManager.LoadScene("Scenes/RoomSceneWithVR");
    }

    public void LoadNewWorld()
    {
        SceneManager.LoadScene("Scenes/WorldEditor");
    }
    
    private void ClickedRoom(GameObject room)
    {
        loadingScript.AddRoom(room);
        loadingScript.loadingtype = LoadingScript.LoadingType.Room;
        SceneManager.LoadScene("Scenes/Room");
    }
    
    private void ClickedWorld(GameObject world)
    {
        loadingScript.AddRoom(world);
        loadingScript.loadingtype = LoadingScript.LoadingType.World;
        SceneManager.LoadScene("Scenes/World");
    }
    
    
}
