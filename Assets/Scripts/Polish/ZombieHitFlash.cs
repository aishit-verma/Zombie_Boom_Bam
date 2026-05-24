using UnityEngine;
using System.Collections;

public class ZombieHitFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.white;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }
}