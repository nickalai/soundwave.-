using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCheck : MonoBehaviour
{

    public Animation anim;
    public TabSelect tabSelect;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.isPlaying) {
            tabSelect.Loading = true;
        }
        else if (!anim.isPlaying) {
            tabSelect.Loading = false;
        }
    }
}
