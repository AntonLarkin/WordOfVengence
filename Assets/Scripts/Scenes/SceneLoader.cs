using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject optionsView;

    private AsyncOperation asyncLoadScene;

    public void OnStartButtonClicked()
    {
        LoadAsync(2);
    }

    public void OnOptionsButtonClicked()
    {
        optionsView.SetActive(true);
    }

    public void OnCloseOptionsViewButtonClicked()
    {
        optionsView.SetActive(false);
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif


    }

    public void  AllowSceneActivation()
    {
        asyncLoadScene.allowSceneActivation = true;
    }

    public void LoadAsync(int sceneIndex, bool allowSceneActivation = true)
    {
        StartCoroutine(OnLoadAsyncInternal(sceneIndex, allowSceneActivation));
    }

    private IEnumerator OnLoadAsyncInternal(int sceneIndex, bool allowSceneActivation)
    {
        asyncLoadScene = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoadScene.allowSceneActivation = allowSceneActivation;

        Debug.Log("here");
        while (!asyncLoadScene.isDone)
        {
            yield return null;
        }
        asyncLoadScene = null;
        Debug.Log("here");
    }

}
