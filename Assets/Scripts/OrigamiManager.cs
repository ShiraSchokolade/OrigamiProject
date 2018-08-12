using HoloToolkit.Examples.InteractiveElements;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class OrigamiManager : MonoBehaviour {

    public GameObject workingArea;
    public GameObject startButton;
    public List<GameObject> origamiStepPrefabs;
    public float distanceToTable = 0.1f;

    private GameObject workingPlane;
    private TapToPlace tapToPlace;
    private GameObject currentShownModel;
    private MeshRenderer workPlaceMesh;

    private Vector3 objectPosition;
    private Quaternion cameraRotationOnPlacement;
    private int currentModelID = 0;

    private bool hasBeenPlaced = false;
    private bool gameStarted = false;
    private bool changedPosition = true;

    void Start () {

        if(workingArea != null)
        {
            workingArea = Instantiate(workingArea);
        }

        if(origamiStepPrefabs.Count == 0)
        {
            Debug.Log("No Prefabs defined");
        }

        workingPlane = workingArea.transform.Find("WorkingPlane").gameObject;
        tapToPlace = workingPlane.GetComponent<TapToPlace>();

        workPlaceMesh = workingArea.transform.Find("Cube").gameObject.GetComponent<MeshRenderer>();
	}
	
	void Update () {

        // placement mode
        if (tapToPlace.IsBeingPlaced)
        {
            changedPosition = true;
            workPlaceMesh.enabled = true;

            if (currentShownModel != null)
                currentShownModel.SetActive(false);

            if(!gameStarted && startButton != null)
                startButton.SetActive(false);
        }
        // static mode
        else
        {
            workPlaceMesh.enabled = false;

            if (changedPosition)
            {
                objectPosition = new Vector3(workingArea.transform.position.x, workingArea.transform.position.y + distanceToTable, workingArea.transform.position.z);
                cameraRotationOnPlacement = Quaternion.Euler(0, CameraCache.Main.transform.localEulerAngles.y, 0);

                if (currentShownModel != null)
                {
                    currentShownModel.SetActive(true);
                    currentShownModel.transform.position = objectPosition;
                }

                if (!gameStarted)
                {
                    startButton.SetActive(true);
                    startButton.transform.position = objectPosition;
                }

                changedPosition = false;
            }

            // work place is placed the first time
            if (!hasBeenPlaced)
            {
                hasBeenPlaced = true;

                if (startButton != null)
                {
                    startButton = Instantiate(startButton, objectPosition, Quaternion.identity);
                    SetButtonEvent();
                }
            }
        }	
	}

    public void ShowNext()
    {
        if (gameStarted)
        {
            if (currentModelID >= origamiStepPrefabs.Count)
                currentModelID = 0;

            if(currentShownModel!= null)
                Destroy(currentShownModel);

            currentShownModel = Instantiate(origamiStepPrefabs[currentModelID], objectPosition, cameraRotationOnPlacement);
            currentModelID++;
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        startButton.SetActive(false);
        ShowNext();
    }

    private void SetButtonEvent()
    {
        UnityEvent e = new UnityEvent();
        e.AddListener(StartGame);
        startButton.transform.Find("Button").gameObject.GetComponent<InteractiveButton>().OnSelectEvents = e;
    }
}
