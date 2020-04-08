using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TabSelect : MonoBehaviour
{
    public int currentTab;
    public TabGroup tabGroup;
    private float zAngle;
    private float targetTime;
    //public UnityEvent ShiftRight;
    //public UnityEvent ShiftLeft;
    // Start is called before the first frame update
    void Start()
    {
        //default tab position is 0;
        currentTab = 0;
        targetTime = 1.0f;

        InvokeRepeating("getInput", 1.0f, targetTime);
    }

    void Update() {
        
        if (Input.GetAxis("Horizontal") < 1 && Input.GetAxis("Horizontal") > -1) {
            targetTime = 1.0f;
        }
        else if (Input.GetAxis("Horizontal") == 1 || Input.GetAxis("Horizontal") == -1) {
            targetTime = targetTime - 0.5f;
        }

        //getInput();
        

        Debug.Log("Input: " + Input.GetAxis("Horizontal"));
        //Debug.Log("Frame Rate: " + Application.targetFrameRate);
    }

    void getInput() {
        //going to the right, positive
        if (Input.GetAxis("Horizontal") > 0) {
            ShiftRight();
            RotateRight();
        }
        //going to the left, negative
        else if (Input.GetAxis("Horizontal") < 0) {
            ShiftLeft();
            RotateLeft();
        }
    }

    public void ShiftRight() {
        //increment by 1 
        if (currentTab >= 0 && currentTab < tabGroup.tabButtons.Count) {
            currentTab++;
        }
        Debug.Log(currentTab);
    }

    public void ShiftLeft() {
        //decrement by 1 
        if (currentTab > 0 && currentTab <= tabGroup.tabButtons.Count) {
            currentTab--;
        }
        Debug.Log(currentTab);
    }

    public void RotateRight() {
        zAngle = (float)(360 / tabGroup.tabButtons.Count) * currentTab;
        this.transform.Rotate(0.0f, 0.0f, zAngle, Space.Self);
    }

    public void RotateLeft() {
        zAngle = (float)(360 / tabGroup.tabButtons.Count) * currentTab;
        this.transform.Rotate(0.0f, 0.0f, zAngle, Space.Self);
    }

}
