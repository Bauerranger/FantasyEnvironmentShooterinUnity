using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GUIBehavior : MonoBehaviour
{
    [SerializeField]
    private Image imageLoad;
    [SerializeField]
    private Image imageBackGround;
    [SerializeField]
    private Button Button1;
    [SerializeField]
    private Button Button2;
    [SerializeField]
    private Button Button3;
    AsyncOperation operation;
    private GameObject NetworkManager;
    private bool menuOpened = false;
    private bool menuOpen = false;
    private bool processingInput = false;
    private bool loads = false;




    private void Start()
    {
        NetworkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        if (imageLoad)
            imageLoad.enabled = false;
    }

    void Update()
    {
            FetchInput();
        UpdateMethods();
        if (loads)
            imageLoad.transform.Rotate(Vector3.forward * 20);
    }

    void FetchInput()
    {
        if (Input.GetKeyDown("escape") && !menuOpen && !processingInput)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>() != null)
                GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>().inputIsActive = false;
            processingInput = true;
            menuOpen = true;
        }

            if (Input.GetKeyDown("escape") && menuOpen && !processingInput)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>() != null)
                GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>().inputIsActive = true;
            processingInput = true;
            menuOpen = false;
        }
    }

    private void UpdateMethods()
    {
        if (menuOpen)
        {
            EventManager.menuMethods += OpenMenu;
            EventManager.menuMethods -= CloseMenu;
            processingInput = false;
        }

        if (!menuOpen)
        {
            EventManager.menuMethods += CloseMenu;
            EventManager.menuMethods -= OpenMenu;
            processingInput = false;
        }
    }

    public void StartHost()
    {
        RotateLoader();
        NetworkServer.Reset();
        NetworkManagerTwo.singleton.StartHost();
    }

    public void StartClient()
    {
        RotateLoader();
        NetworkManagerTwo.singleton.StartClient();
    }

    public void TurnOffMenu()
    {
        if (GameObject.FindGameObjectWithTag("inGame_Menu").GetComponent<Canvas>() != null)
            GameObject.FindGameObjectWithTag("inGame_Menu").GetComponent<Canvas>().enabled = false;
    }

    public void TurnOnMenu()
    {
        if (GameObject.FindGameObjectWithTag("inGame_Menu").GetComponent<Canvas>() != null)
            GameObject.FindGameObjectWithTag("inGame_Menu").GetComponent<Canvas>().enabled = true;
        
    }

    public void QuitApplication()
    {

        gameObject.GetComponent<Canvas>().enabled = false;
        Application.Quit();
    }

    public void Disconnect()
    {
        if (NetworkServer.active)
        {
            NetworkManagerTwo.singleton.StopHost();
        }
        else
        {
            if (NetworkClient.active)
                NetworkManagerTwo.singleton.StopClient();
        }
    }

    public void OpenMenu()
    {
        if (!menuOpened)
        {
            EventManager.menuMethods -= TurnOffMenu;
            Cursor.visible = true;
            EventManager.menuMethods += TurnOnMenu;
            menuOpened = true;
        }
    }

    public void CloseMenu()
    {
        if (menuOpened)
        {
            EventManager.menuMethods -= TurnOnMenu;
            Cursor.visible = false;
            EventManager.menuMethods += TurnOffMenu;
            menuOpened = false;
        }
    }

    public void CloseByButton()
    {
        menuOpen = false;
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>() != null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>().inputIsActive = true;
    }

    public void RotateLoader()
    {

        Vector3 rotateLoad = new Vector3(0, 0, 0.1f);
        if (imageBackGround != null)
            imageBackGround.enabled = false;
        if (Button1 != null)
            Button1.gameObject.SetActive(false);
        if (Button2 != null)
            Button2.gameObject.SetActive(false);
        if (Button3 != null)
            Button3.gameObject.SetActive(false);
        if (imageLoad != null)
            imageLoad.enabled = true;
        loads = true;
    }

}