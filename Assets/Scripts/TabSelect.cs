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
    public int tabs;

    public List<Quaternion> rotList = new List<Quaternion>();

    private float targetAngle = 0;
    private float angleAmount;
    public bool loading = false;
    public bool isIncrementing = false;

    public AnimCheck animCheck;

    //sound
    public AudioClip sound;
    public AudioSource source { get { return GetComponent<AudioSource>(); } }


    // Start is called before the first frame update
    void Start(){
        //default tab position is 0;
        currentTab = 0;
        tabs = tabGroup.objectsToSwap.Count;
        angleAmount = 360 / tabs;

        Debug.Log("current Tab = " + currentTab);

        CreateQuatList();

        source.clip = sound;
        source.playOnAwake = false;
    }

    private void Update() {
       // Debug.Log(Input.GetAxis("Horizontal"));

        if (!isIncrementing) {
            InvokeRepeating("GetInput", 0, 0.3f);
            isIncrementing = true;
        }

        /*
        if (Input.GetAxis("Horizontal") < 0.05 && Input.GetAxis("Horizontal") > -0.05 ) {
            GetInput();
        }
        */
    }

    void CreateQuatList() {
        for (int i = 0; i < tabs; ++i) {
            targetAngle = angleAmount * i;
            //Debug.Log(targetAngle);
            rotList.Add(Quaternion.Euler(new Vector3(0, 0, targetAngle)));
        }
    }

    public void GetInput() {
        loading = true;
        //going to the right, positive
        if (Input.GetAxis("Mouse X")  > 0) {
            ShiftRight();
        }
        //going to the left, negative
        else if (Input.GetAxis("Mouse X") < 0) {
            ShiftLeft();
        }   
    }

    public void ShiftRight() {
        //increment by 1 
        int prevTab = currentTab;
        if (currentTab > -1 && currentTab < tabs -1) {
            PlaySound();
            currentTab++;
            //transform.rotation = Quaternion.Slerp(rotList[prevTab], rotList[currentTab], 0.2f);
            transform.Rotate(Vector3.forward * Time.deltaTime * 60);
        }
    }

    public void ShiftLeft() {
        //decrement by 1 
        int prevTab = currentTab;
        if (currentTab > 0 && currentTab < tabs) {
            PlaySound();
            currentTab--;
            //transform.rotation = Quaternion.Slerp(rotList[prevTab], rotList[currentTab], 0.2f);
            transform.Rotate(-Vector3.forward * Time.deltaTime * 60);
        }
    }

    void PlaySound() {
        source.PlayOneShot(sound);
    }
}
