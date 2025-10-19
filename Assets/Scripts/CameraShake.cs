using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    
    // Original position of the camera
    private Vector3 originalPosition;
    
    // Shake parameters
    [SerializeField] private float defaultShakeDuration = 0.15f;
    [SerializeField] private float defaultShakeMagnitude = 0.1f;
    
    // Keep track of current shake coroutine
    private Coroutine currentShake;
    
    void Awake()
    {
        instance = this;
        originalPosition = transform.localPosition;
    }
    
    /// Shake the camera with default parameters
    public void ShakeCamera()
    {
        ShakeCamera(defaultShakeDuration, defaultShakeMagnitude);
    }
    
    /// Shake the camera with custom parameters
    public void ShakeCamera(float duration, float magnitude)
    {
        // Stop any current shake
        if (currentShake != null)
        {
            StopCoroutine(currentShake);
        }
        
        // Start a new shake
        currentShake = StartCoroutine(Shake(duration, magnitude));
    }
    
    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            // Calculate shake amount based on remaining time (tapered effect)
            float percentComplete = elapsed / duration;
            float dampingFactor = 1f - Mathf.Clamp01(percentComplete);
            
            // Generate random offset
            float x = Random.Range(-1f, 1f) * magnitude * dampingFactor;
            float y = Random.Range(-1f, 1f) * magnitude * dampingFactor;
            
            // Apply offset to camera
            transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Reset camera position
        transform.localPosition = originalPosition;
        currentShake = null;
    }
}