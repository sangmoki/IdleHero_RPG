using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    GameObject sliderParent;
    public Slider slider;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI versionText;
    public GameObject tabToStart;
    public GameObject loadingBar;
    private AsyncOperation asyncOperation;


    private void Start()
    {
        versionText.text = "Version " + Application.version;
        sliderParent = slider.transform.parent.gameObject;
        StartCoroutine(LoadDataCoroutine());
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

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
        percentageText.text = string.Format("데이터를 가져오고 있습니다..<color=#FFFF00>{0}%", progress * 100.0f);
    }
}
