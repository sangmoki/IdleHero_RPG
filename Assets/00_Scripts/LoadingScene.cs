using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    GameObject sliderParent;
    public Slider slider;
    public TextMeshProUGUI versionText, percentText;
    public GameObject tabToStart;
    public GameObject loadingBar;
    private AsyncOperation asyncOperation;


    private void Start()
    {
        versionText.text = "Version " + Application.version;
        sliderParent = slider.transform.parent.gameObject;
    }

    private void Update()
    {
        // asyncOperation이 null이 아닐 때 (즉, Main 씬을 가지고 
        if (asyncOperation != null)
        {
            // 데이터 로드가 90퍼 이상이고 마우스 클릭이 되었다면 게임 씬으로 전환
            if (asyncOperation.progress >= 0.9f && Input.GetMouseButtonDown(0))
            {
                asyncOperation.allowSceneActivation = true;
            }
        }

    }

    IEnumerator LoadDataCoroutine()
    {
        // 씬을 미리 로드하여 메모리에 올려놓음
        asyncOperation = SceneManager.LoadSceneAsync("Main");
        // 씬을 로드하지만 활성화는 하지 않음 (자동 전환 방지)
        asyncOperation.allowSceneActivation = false;

        // progress가 0.9f 이상이 되면 로딩이 완료된 것으로 간주
        while (asyncOperation.progress < 0.9f)
        {
            LoadingUpdate(asyncOperation.progress);
            yield return null;
        }
        LoadingUpdate(1.0f);
        loadingBar.SetActive(false);
        tabToStart.SetActive(true);
        
    }

    public void LoadingMain()
    {
        StartCoroutine(LoadDataCoroutine());
    }

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
        percentText.text = string.Format("{0}%", progress * 100.0f);
    }
}
