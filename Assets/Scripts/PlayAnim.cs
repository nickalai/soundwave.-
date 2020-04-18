using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public List<GameObject> animList;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Play() {
        foreach (var obj in animList) {
            obj.SetActive(true);
        }
    }
}
