using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPart : MonoBehaviour
{
    private Material mat;
    private Color emissionColor;
    public float pulseSpeed = 0.2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 5f;

    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private void OnEnable()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = Color.red;
        mat.EnableKeyword("_EMISSION");
        
        ColorUtility.TryParseHtmlString("#242424", out emissionColor);
    }

    private void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, 
            Mathf.PingPong(Time.time * pulseSpeed, 1));
        mat.SetColor("_EmissionColor", emissionColor * intensity);
    }

    public void HitDeathPart()
    {
        AudioManager.instance?.PlayDeathExplosionSound();

        if (CameraShake.instance != null)
        {
            CameraShake.instance.ShakeCamera(shakeDuration, shakeMagnitude);
        }
        
        // Add a small delay before restarting to allow the shake to be visible
        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        // Wait a short moment for the camera shake to be noticeable
        // This is shorter than the full shake duration to keep the game responsive
        yield return new WaitForSeconds(0.1f);
        
        // Make sure the ball's super speed is disabled before restarting
        BallController ball = FindObjectOfType<BallController>();
        if (ball != null)
        {
            ball.isSuperSpeedActive = false;
        }

        if (GameManager.singleton != null && GameManager.singleton.currentGameMode == GameMode.Survival)
        {
            GameManager.singleton.GameOver();
            yield break; // Exit the coroutine early
        } else {
            // Restart the level
            GameManager.singleton.Restartlevel();
        }
    }
}