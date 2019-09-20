using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;
    

    public void screen_shake(float ss_values_x, float ss_values_y)
    {
        StopCoroutine(screen_shake2(ss_values_x, ss_values_y));
        StartCoroutine(screen_shake2(ss_values_x, ss_values_y));
    }

    public IEnumerator screen_shake2(float time, float magnitude)
    {
        Vector3 start_vec = Vector3.forward * -10f;
        float time_left = 0.0f;
        while(time_left < time)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, start_vec.z), animationCurve.Evaluate(time_left/time));
            time_left += Time.deltaTime;
            yield return null;
        }

        transform.position = start_vec;
    }
}
