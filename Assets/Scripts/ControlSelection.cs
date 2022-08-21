using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlSelection : MonoBehaviour {

    private GameObject lastSelected = null;
    
    [SerializeField]
    private GameObject mainContainer = null;

    private bool lastMainContainerActive = true;

    private void Update() {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected == null || currentSelected.name == "Main Menu Button")
            EventSystem.current.SetSelectedGameObject(lastSelected);
        if (MainContainerActivated())
            StartCoroutine(SelectAfterFrame(lastSelected));
        lastSelected = EventSystem.current.currentSelectedGameObject;
        lastMainContainerActive = mainContainer.activeInHierarchy;
    }

    private bool MainContainerActivated() {
        return mainContainer.activeInHierarchy && !lastMainContainerActive;
    }

    private IEnumerator SelectAfterFrame(GameObject lastSelected) {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(lastSelected);
    }

}