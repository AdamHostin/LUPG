using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSinMovement : MonoBehaviour
{
    [SerializeField] float sinScale;
    [SerializeField] float sinWidth;

    private float timer = 0f;

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        //if (timer > 2 * Mathf.PI) timer -= (Mathf.PI * 2);

        transform.localPosition = new Vector3(transform.localPosition.x, Oscillate(), transform.localPosition.z);
    }

    float Oscillate()
    {
        return Mathf.Cos(sinWidth * timer / Mathf.PI) * sinScale;
    }

    public void ResetTimer() => timer = 0f;
}
