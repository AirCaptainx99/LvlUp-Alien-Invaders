using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        if (this != Instance)
        {
            Destroy(this);
        }
    }

    public void ChangeScene(int idxBuild)
    {
        SceneManager.LoadScene(idxBuild, LoadSceneMode.Single);

        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.DestroyAllGameObjectInPool();
        }
    }

    public void RestartScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuScene()
    {
        ChangeScene(0);
    }
}
