using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabSelect : MonoBehaviour
{
    public int currentTab;
    public TabGroup tabGroup;

    List<Quaternion> rotList = new List<Quaternion>();
    private float targetAngle = 0;
    private float angleAmount;
    public bool Loading = false;


    // Start is called before the first frame update
    void Start(){
        //default tab position is 0;
        currentTab = 0;

        angleAmount = 360 / tabGroup.objectsToSwap.Count;
        //Debug.Log(angleAmount);
        //Debug.Log(tabGroup.objectsToSwap.Count);

        CreateQuatList();
        
        //StartCoroutine(GetInput());
    }

    private void Update() {

        /*
        if (anim.isPlaying) {
            Loading = true;
        }
        else if (!anim.isPlaying) {
            Loading = false;
        }
        */

        GetInput();
        
    }

    public void CreateQuatList() {
        for (int i = 0; i < tabGroup.tabButtons.Count; i++) {
            rotList.Add(Quaternion.Euler(new Vector3(0, 0, targetAngle)));
            targetAngle = angleAmount*i;
            //Debug.Log(rotList[i]);
        }
    }

    public void GetInput() {
        //going to the right, positive
        //Debug.Log("Input: " + Input.GetAxis("Horizontal"));

        if (Input.GetAxis("Horizontal") == 1) {
            ShiftRight();
            Rotate();
        }
        //going to the left, negative
        else if (Input.GetAxis("Horizontal") == -1) {
            ShiftLeft();
            Rotate();
        }   
    }

    public void ShiftRight() {
        //increment by 1 
        if (currentTab >= 0 && currentTab < tabGroup.tabButtons.Count) {
            currentTab++;
        }
        //Debug.Log(currentTab);
    }

    public void ShiftLeft() {
        //decrement by 1 
        if (currentTab > 0 && currentTab <= tabGroup.tabButtons.Count) {
            currentTab--;
        }
       // Debug.Log(currentTab);
    }

    public void Rotate() {
        if (currentTab > 0 && currentTab < tabGroup.tabButtons.Count) {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotList[currentTab], 0.2f);
        }
        //Debug.Log(rotList[currentTab]);
    }

    /*
    IEnumerator Hold() {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(2);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    */
   

}
