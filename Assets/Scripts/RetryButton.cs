using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RetryButton : MonoBehaviour
{
    public void GoToEntrance()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        GameObject temp = new GameObject("Temp");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;

        GameObject[] rootObjects = ddolScene.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; i++)
        {
            if (rootObjects[i] != temp)
            {
                Destroy(rootObjects[i]);
            }
        }

        yield return null;
        SceneManager.LoadScene("Entrance");
    }
}