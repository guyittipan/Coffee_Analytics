using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameVisualController : MonoBehaviour
{
    [SerializeField] private float delayBeforeFall = 0.2f;
    [SerializeField] private float delayBetweenItems = 0.05f;
    [SerializeField] private float waitAfterAllFall = 1.0f;

    public void PlayEndSequenceAndLoadScore()
    {
        StartCoroutine(EndSequenceRoutine());
    }

    private IEnumerator EndSequenceRoutine()
    {
        yield return new WaitForSeconds(delayBeforeFall);

        Ingredient[] ingredients = FindObjectsOfType<Ingredient>();

        for (int i = 0; i < ingredients.Length; i++)
        {
            if (ingredients[i] != null)
            {
                IngredientFallEffect effect = ingredients[i].GetComponent<IngredientFallEffect>();
                if (effect == null)
                {
                    effect = ingredients[i].gameObject.AddComponent<IngredientFallEffect>();
                }

                effect.PlayFall();
                yield return new WaitForSeconds(delayBetweenItems);
            }
        }

        yield return new WaitForSeconds(waitAfterAllFall);

        if (GameController.I != null)
        {
            GameController.I.LoadScore();
        }
    }
}