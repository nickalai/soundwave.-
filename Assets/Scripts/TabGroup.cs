using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;
    public TabSelect tabSelect;

    public ChartDisplay chartDis;

    LoadChart lc;

    //change sprites
    public Sprite tabActive;
    public Sprite tabIdle;

    public void Start() {
        lc = GameObject.FindGameObjectWithTag("ChartLoader").GetComponent<LoadChart>();
    }

    public void Subscribe(TabButton button) {
        if(tabButtons == null) { 
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
        tabButtons.Sort((e1, e2) => e1.transform.GetSiblingIndex().CompareTo(e2.transform.GetSiblingIndex()));
    }

    //these change the state of the tabs
    
    public void OnTabEnter(TabButton button) {

    }

    public void OnTabExit(TabButton button) {

    }

    public void OnTabSelected(TabButton button) {

        if(selectedTab != null) {
            selectedTab.Deselect();
        }

        //assign selected tab 
        selectedTab = button;

        selectedTab.Select();

        //order of objects in this list should match the button order
        int index = button.transform.GetSiblingIndex();

        //iterate through game objects
        for (int i=0; i < objectsToSwap.Count; i++) {
            if (i == index) {
                objectsToSwap[i].SetActive(true);
                chartDis = objectsToSwap[i].GetComponent<ChartDisplay>();
                lc.setChart(chartDis.chart);
               
                //lc.chartToLoad = objectsToSwap[i].GetComponent<ChartDisplay>().chart.koreo;
                //lc.eventIDToLoad = objectsToSwap[i].GetComponent<ChartDisplay>().chart.eventID;
            }
            else {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs() {

    }

}
