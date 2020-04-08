using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup;
    public int songID;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    public TabSelect tabSelect;

    //public Image background;

    // Start is called before the first frame update
    void Start()
    {
        tabGroup.Subscribe(this);
    }

    void Update() {
        if (tabSelect.currentTab == songID) {
            tabGroup.OnTabSelected(this);
        }
    }

    public void Select() {
        if (onTabSelected != null) {
            onTabSelected.Invoke();
        }
    }

    public void Deselect() {
        if (onTabDeselected != null) {
            onTabDeselected.Invoke();
        }
    }
}
