using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIBehavior : MonoBehaviour
{
    [SerializeField]
    private Image imageLoad;
    [SerializeField]
    private Image imageBackGroundMainMenue;
    [SerializeField]
    private Button Button1MainMenue;
    [SerializeField]
    private Button Button2MainMenue;
    [SerializeField]
    private Button Button3MainMenue;
    [SerializeField]
    private Image imageBackGroundInGameMenue;
    [SerializeField]
    private Button Button4InGameMenue;
    [SerializeField]
    private Button Button5InGameMenue;
    [SerializeField]
    private Button Button6InGameMenue;
    AsyncOperation operation;
    private GameObject NetworkManager;
    private bool menuOpened = false;
    private bool menuOpen = false;
    private bool processingInput = false;
    private bool loads = false;
    private bool menueIsSetForScene = false;
    private int oldScene = 99;




    private void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        NetworkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        if (imageLoad)
            imageLoad.enabled = false;
            imageBackGroundInGameMenue.enabled = false;
            Button4InGameMenue.enabled = false;
            Button5InGameMenue.enabled = false;
            Button6InGameMenue.enabled = false;
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        if (oldScene != buildIndex)
        {
            if (buildIndex == 0)
            {
                if (imageBackGroundInGameMenue)
                    imageBackGroundInGameMenue.enabled = true;
                if (Button4InGameMenue)
                    Button4InGameMenue.enabled = true;
                if (Button5InGameMenue)
                    Button5InGameMenue.enabled = true;
                if (Button6InGameMenue)
                    Button6InGameMenue.enabled = true;
                oldScene = buildIndex;
            }
            if (buildIndex > 0)
            {
                loads = false;
                if (imageLoad)
                    imageLoad.enabled = false;
                if (imageBackGroundInGameMenue)
                    imageBackGroundInGameMenue.enabled = true;
                if (Button4InGameMenue)
                    Button4InGameMenue.enabled = true;
                if (Button5InGameMenue)
                    Button5InGameMenue.enabled = true;
                if (Button6InGameMenue)
                    Button6InGameMenue.enabled = true;
                oldScene = buildIndex;
            }
        }

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
        if (gameObject.GetComponent<Canvas>() != null)
            gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void TurnOnMenu()
    {
        if (gameObject.GetComponent<Canvas>() != null)
            gameObject.GetComponent<Canvas>().enabled = true;
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
        if (imageBackGroundMainMenue != null)
            imageBackGroundMainMenue.enabled = false;
        if (Button1MainMenue != null)
            Button1MainMenue.gameObject.SetActive(false);
        if (Button2MainMenue != null)
            Button2MainMenue.gameObject.SetActive(false);
        if (Button3MainMenue != null)
            Button3MainMenue.gameObject.SetActive(false);
        if (imageLoad != null)
            imageLoad.enabled = true;
        loads = true;
    }

    public void showLooseScreen()
    {
        if (imageLoad)
            imageLoad.enabled = false;
        if (imageBackGroundInGameMenue)
            imageBackGroundInGameMenue.enabled = false;
        if (Button4InGameMenue)
            Button4InGameMenue.enabled = false;
        if (Button5InGameMenue)
            Button5InGameMenue.enabled = false;
        if (Button6InGameMenue)
            Button6InGameMenue.enabled = false;
    }

    public void showWinScreen()
    {
        if (imageLoad)
            imageLoad.enabled = false;
        if (imageBackGroundInGameMenue)
            imageBackGroundInGameMenue.enabled = false;
        if (Button4InGameMenue)
            Button4InGameMenue.enabled = false;
        if (Button5InGameMenue)
            Button5InGameMenue.enabled = false;
        if (Button6InGameMenue)
            Button6InGameMenue.enabled = false;
    }
}