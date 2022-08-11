using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public IEnumerator ScreenShake(float magnitude, float duration)
    {
        while (duration > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y + y, transform.localPosition.z);

            duration -= Time.deltaTime;

            if (Time.timeScale > 0)
            {
                duration -= Time.deltaTime;
            }
            else
            {
                duration -= 0.01f;
            }
            yield return null;
        }
        transform.position = startPos;
    }
}
