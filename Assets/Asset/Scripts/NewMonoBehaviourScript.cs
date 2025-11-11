using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class RandomRotate : MonoBehaviour
{
    [Header("Speed (degrees/second)")]
    public float minSpeed = 30f;
    public float maxSpeed = 120f;

    [Header("Time between direction changes (seconds)")]
    public float changeIntervalMin = 0.5f;
    public float changeIntervalMax = 3f;

    [Header("Smoothing")]
    [Tooltip("How fast current speed moves towards target speed (higher = snappier)")]
    public float speedSmooth = 5f;

    // internal (kunci sumbu X)
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private Coroutine changerCoroutine;

    void OnEnable()
    {
        if (changerCoroutine != null) StopCoroutine(changerCoroutine);
        changerCoroutine = StartCoroutine(ChangeRoutine());
    }

    void OnDisable()
    {
        if (changerCoroutine != null) StopCoroutine(changerCoroutine);
        changerCoroutine = null;
    }

    IEnumerator ChangeRoutine()
    {
        SetRandomTarget();
        while (true)
        {
            float wait = Random.Range(changeIntervalMin, changeIntervalMax);
            yield return new WaitForSeconds(wait);
            SetRandomTarget();
        }
    }

    void SetRandomTarget()
    {
        int dir = Random.value < 0.5f ? -1 : 1;
        float speed = Random.Range(minSpeed, maxSpeed);
        targetSpeed = dir * speed;
    }

    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 1f - Mathf.Exp(-speedSmooth * Time.deltaTime));

        // 🔒 hanya sumbu X
        transform.Rotate(Vector3.right * currentSpeed * Time.deltaTime, Space.Self);
    }
}
