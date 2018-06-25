using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour {

    public List<GameObject> targetPrefabs;
    private List<GameObject> targetList;

    private int currentTargetID;

    void Start () {

        targetList = new List<GameObject>();
        GameObject t;

        foreach (GameObject target in targetPrefabs)
        {
            t = Instantiate(target);
            //t.SetActive(false);
            targetList.Add(t);
        }

        if(targetList.Count > 0)
        {
            targetList[0].SetActive(true);
            currentTargetID = 0;
        }
        else
        {
            Debug.Log("Target List is empty");
        }

    }
	
	void Update () {
		
	}

    public void ShowNext()
    {

        if (currentTargetID + 1 < targetList.Count)
        {
            // hide current target
            targetList[currentTargetID].SetActive(false);

            // show next target
            targetList[currentTargetID + 1].SetActive(true);
            currentTargetID = currentTargetID + 1;
        }
        else if(targetList.Count > 0)
        {
            // hide current target
            targetList[currentTargetID].SetActive(false);

            // show first target again
            targetList[0].SetActive(true);
            currentTargetID = 0;
        }
    }

    public void PlayCamera()
    {
        CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_DEFAULT);

        CameraDevice.Instance.Start();
    }

    public void StopCamera()
    {
        CameraDevice.Instance.Stop();

        CameraDevice.Instance.Deinit();
    }
}
