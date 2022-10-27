using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup[] windows;

    List<Tab> tabs = new List<Tab>();

    [SerializeField]
    GameObject tabPrefab;

    HorizontalLayoutGroup layout;

    private void Start()
    {
        layout = GetComponent<HorizontalLayoutGroup>();

        for(int i = 0; i < windows.Length; i++)
        {
            GameObject newTab = Instantiate(tabPrefab);
            Tab tabLogic = newTab.GetComponent<Tab>();
            tabLogic.Setup(windows[i].gameObject.name, i, this);

            newTab.transform.SetParent(transform);
            newTab.transform.position = transform.position;

            tabs.Add(tabLogic);

            windows[i].gameObject.SetActive(true);
        }
        SwitchTo(0);
        Invoke("Align", 0.05f);
    }

    void Align()
    {
        layout.spacing++;
        layout.spacing--;
    }

    public void SwitchTo(int index)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            tabs[i].Deactivate();

            windows[i].alpha = 0;
            windows[i].interactable = false;
            windows[i].blocksRaycasts = false;
        }

        tabs[index].Activate();
        windows[index].alpha = 1;
        windows[index].interactable = true;
        windows[index].blocksRaycasts = true;
    }
}
