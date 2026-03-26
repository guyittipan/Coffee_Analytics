using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngredientFallEffect : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 30f;
    [SerializeField] private float jumpTime = 0.15f;
    [SerializeField] private float fallDistance = 1200f;
    [SerializeField] private float fallTime = 0.8f;
    [SerializeField] private float randomXOffset = 80f;
    [SerializeField] private float randomRotation = 25f;

    private RectTransform rt;
    private Vector2 startPos;
    private float startRotZ;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            startPos = rt.anchoredPosition;
            startRotZ = rt.localEulerAngles.z;
        }
    }

    public void PlayFall()
    {
        StopAllCoroutines();
        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        if (rt == null) yield break;

        Vector2 upPos = startPos + new Vector2(0f, jumpHeight);

        float t = 0f;
        while (t < jumpTime)
        {
            float lerp = t / jumpTime;
            rt.anchoredPosition = Vector2.Lerp(startPos, upPos, lerp);
            t += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = upPos;

        Vector2 endPos = upPos + new Vector2(Random.Range(-randomXOffset, randomXOffset), -fallDistance);
        float endRot = startRotZ + Random.Range(-randomRotation, randomRotation);

        t = 0f;
        Vector2 fallStart = rt.anchoredPosition;
        float rotStart = rt.localEulerAngles.z;

        while (t < fallTime)
        {
            float lerp = t / fallTime;
            float eased = lerp * lerp;

            rt.anchoredPosition = Vector2.Lerp(fallStart, endPos, eased);
            float z = Mathf.LerpAngle(rotStart, endRot, lerp);
            rt.localRotation = Quaternion.Euler(0f, 0f, z);

            t += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = endPos;
        rt.localRotation = Quaternion.Euler(0f, 0f, endRot);
    }
}