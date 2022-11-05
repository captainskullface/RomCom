using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InputName : MonoBehaviour
{
    [SerializeField] TMP_InputField inputs;
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI loading;
    [SerializeField] TextMeshProUGUI hint;
    [SerializeField] float writeWaitTime;
    [SerializeField] float loadingTime;
    [SerializeField] Slider loadingBar;
    [SerializeField] float barTime;
    [SerializeField] TextMeshProUGUI inputName;
    [SerializeField] public static string companyName;
    bool isLoading;
    // Start is called before the first frame update
    void Start()
    {
        inputs.gameObject.SetActive(true);
        username.gameObject.SetActive(true);
        loading.gameObject.SetActive(false);
        hint.gameObject.SetActive(true);
        loadingBar.value = loadingBar.minValue;
        loadingBar.gameObject.SetActive(false);
        isLoading = false;

    }


    // Update is called once per frame
    void Update()
    {
        companyName = inputName.text;
        if (loadingBar.value == loadingBar.maxValue)
        {
            SceneManager.LoadScene(1);
        }
        if (isLoading)
        {
            loadingBar.value += barTime * Time.deltaTime;
        }
    }
    public void LoadScreen()
    {
        inputs.gameObject.SetActive(false);
        username.gameObject.SetActive(false);
        loading.gameObject.SetActive(true);
        hint.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(true);
        isLoading = true;
        StartCoroutine(BootingUp());
        

    }
    IEnumerator BootingUp()
    {
        while (true)
        {
            for (int i = 0; i < loading.text.Length; i++)
            {
                loading.maxVisibleCharacters = i + 1;
                yield return new WaitForSeconds(writeWaitTime);
            }

            yield return new WaitForSeconds(loadingTime);

        
        }
    }



}