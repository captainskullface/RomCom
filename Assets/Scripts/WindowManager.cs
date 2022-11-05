using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup[] windows;

    [SerializeField]
    Sprite[] icons;

    List<App> apps = new List<App>();

    [SerializeField]
    GameObject iconPrefab;

    HorizontalLayoutGroup layout;

    private void Start()
    {
        layout = GetComponent<HorizontalLayoutGroup>();

        for(int i = 0; i < windows.Length; i++)
        {
            GameObject newTab = Instantiate(iconPrefab);
            App tabLogic = newTab.GetComponent<App>();
            tabLogic.Setup(windows[i].gameObject.name, i, this, icons[i]);

            newTab.transform.SetParent(transform);
            newTab.transform.position = transform.position;

            apps.Add(tabLogic);

            windows[i].gameObject.SetActive(true);

            ToggleWindow(i, true);
        }
        //OpenApp(0);
        Invoke("Align", 0.05f);
    }

    void Align()
    {
        layout.spacing++;
        layout.spacing--;
    }

    public void ClickApp(int index)
    {
        /*
        for (int i = 0; i < windows.Length; i++)
        {
            apps[i].Deactivate();

            windows[i].alpha = 0;
            windows[i].interactable = false;
            windows[i].blocksRaycasts = false;
        }

        apps[index].Activate();
        windows[index].alpha = 1;
        windows[index].interactable = true;
        windows[index].blocksRaycasts = true;
        */

        //RectTransform appRect = windows[index].gameObject.GetComponent<RectTransform>();

        if(apps[index].active)
        {
            if (windows[index].transform.parent.GetChild(windows[index].transform.parent.childCount - 1) == windows[index].transform)
            {
                ToggleWindow(index);
            }
            else
            {
                windows[index].transform.SetAsLastSibling();
            }
        }
        else
        {
            ToggleWindow(index);
        }
    }

    void ToggleWindow(int index, bool instant = false)
    {
        //windows[index].alpha = windows[index].alpha == 1 ? 0 : 1;
        //windows[index].interactable = !windows[index].interactable;
        //windows[index].blocksRaycasts = !windows[index].blocksRaycasts;

        RectTransform appRect = windows[index].GetComponent<RectTransform>();

        float animTime = 0.25f;
        if (instant)
            animTime = 0.001f;

        float windowRatio = 0.8f;

        if(apps[index].active || instant)
        {
            //Tweener open = DOTweenModuleUI.DOSizeDelta(appRect, Vector3.zero, animTime);
            appRect.DOMove(apps[index].transform.position, animTime);
            appRect.DOScale(0, animTime);
            windows[index].DOFade(0, animTime);
            apps[index].Deactivate();
            StartCoroutine(SendToBack(index, animTime));
            windows[index].interactable = false;
            windows[index].blocksRaycasts = false;
        }
        else
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height - ((Screen.height * windowRatio) / 8), 0);
            appRect.DOMove(screenCenter, animTime);
            appRect.DOScale(windowRatio, animTime);
            windows[index].DOFade(1, animTime);
            windows[index].transform.SetAsLastSibling();
            apps[index].Activate();

            windows[index].interactable = true;
            windows[index].blocksRaycasts = true;
        }    
    }

    IEnumerator SendToBack(int index, float waitTime)
    {
        yield return new WaitForSeconds(waitTime * 0.75f);

        if (!apps[index].active)
            windows[index].transform.SetAsFirstSibling();
    }
}
