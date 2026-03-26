using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HardResetGame : MonoBehaviour
{
    public void ResetGameLikeNew()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        // หา scene พิเศษ DontDestroyOnLoad
        GameObject temp = new GameObject("Temp");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;

        // ลบทุก object ใน DontDestroyOnLoad
        GameObject[] rootObjects = ddolScene.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; i++)
        {
            if (rootObjects[i] != temp)
            {
                Destroy(rootObjects[i]);
            }
        }

        yield return null;

        // โหลดฉากเริ่มต้นใหม่
        SceneManager.LoadScene("Entrance");
    }
}