using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLobby : MonoBehaviour
{
    AsyncOperation asyncOperation;
    [SerializeField] private Image loadBar;
    [SerializeField] private Text barText;
    [SerializeField] private int SceneID;

    private void Start()
    {
        StartCoroutine(OnLoadScene());
        
    }

    private IEnumerator OnLoadScene()
    {
        yield return new WaitForSeconds(1f);
        asyncOperation = SceneManager.LoadSceneAsync(SceneID);
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            loadBar.fillAmount = progress;
            barText.text = "Loading " + progress*100 + "%";
            yield return 0;
        }
    }
}
