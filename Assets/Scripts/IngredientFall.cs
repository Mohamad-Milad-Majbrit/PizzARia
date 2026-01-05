using UnityEngine;
using System.Collections;

public class IngredientFall : MonoBehaviour
{
    public AnimationCurve fallCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float duration = 0.5f;

    public void StartDropAnimation(Vector3 targetPos, float dropHeight)
    {
        StartCoroutine(FallRoutine(targetPos, dropHeight));
    }

    IEnumerator FallRoutine(Vector3 targetPos, float dropHeight)
    {
        Vector3 startPos = targetPos + Vector3.up * dropHeight;
        float elapsed = 0f;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float curveValue = fallCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, targetPos, curveValue);

            yield return null;
        }

        transform.position = targetPos;
        this.enabled = false;
    }
}