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
        fetchInput();
        updateMethods();
        if (loads)
            imageLoad.transform.Rotate(Vector3.forward * 20);
    }

    void fetchInput()
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

    private void updateMethods()
    {
        if (menuOpen)
        {
            EventManager.menuMethods += openMenu;
            EventManager.menuMethods -= closeMenu;
            processingInput = false;
        }

        if (!menuOpen)
        {
            EventManager.menuMethods += closeMenu;
            EventManager.menuMethods -= openMenu;
            processingInput = false;
        }
    }

    public void startHost()
    {
        rotateLoader();
        NetworkServer.Reset();
        NetworkManagerTwo.singleton.StartHost();
    }

    public void startClient()
    {
        rotateLoader();
        NetworkManagerTwo.singleton.StartClient();
    }

    public void turnOffMenu()
    {
        if (gameObject.GetComponent<Canvas>() != null)
            gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void turnOnMenu()
    {
        if (gameObject.GetComponent<Canvas>() != null)
            gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void quitApplication()
    {

        gameObject.GetComponent<Canvas>().enabled = false;
        Application.Quit();
    }

    public void disconnect()
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

    public void openMenu()
    {
        if (!menuOpened)
        {
            EventManager.menuMethods -= turnOffMenu;
            Cursor.visible = true;
            EventManager.menuMethods += turnOnMenu;
            menuOpened = true;
        }
    }

    public void closeMenu()
    {
        if (menuOpened)
        {
            EventManager.menuMethods -= turnOnMenu;
            Cursor.visible = false;
            EventManager.menuMethods += turnOffMenu;
            menuOpened = false;
        }
    }

    public void closeByButton()
    {
        menuOpen = false;
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>() != null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerController>().inputIsActive = true;
    }

    public void rotateLoader()
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