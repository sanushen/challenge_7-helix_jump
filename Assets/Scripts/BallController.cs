using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextColliison;
    public Rigidbody rb; 
    public float impulseForce = 5f;
    public Vector3 startPos;
    public int perfectPass = 0;
    public bool isSuperSpeedActive;
    public TrailRenderer trailRenderer; 
    private bool gameStarted = false;

    [SerializeField] private GameObject bounceParticlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        // Disable physics until game starts
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void GameStarted()
    {
        gameStarted = true;
        // Enable physics when game starts
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(ignoreNextColliison)
            return;

        AudioManager.instance?.PlayBounceSound();


        //Bounce Particles
        if (bounceParticlePrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            
            // Find the actual platform segment that was hit
            Transform platformSegment = collision.transform;
            
            // Instantiate the particles
            GameObject particleObj = Instantiate(bounceParticlePrefab);
            
            // Parent the particles to the platform segment
            particleObj.transform.SetParent(platformSegment);
            
            // Convert the contact point to local space relative to the platform segment
            Vector3 localContactPoint = platformSegment.InverseTransformPoint(contact.point);
            
            // Set the local position of the particles
            particleObj.transform.localPosition = localContactPoint;
            
            // Make particles face away from the collision surface
            Vector3 localNormal = platformSegment.InverseTransformDirection(contact.normal);
            particleObj.transform.localRotation = Quaternion.LookRotation(localNormal);
            
            // Get the particle system and play it
            ParticleSystem particles = particleObj.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
                
                // Destroy after the particles finish
                float duration = particles.main.duration + particles.main.startLifetime.constantMax;
                Destroy(particleObj, duration);
            }
        }

        if(isSuperSpeedActive) {
            if(!collision.transform.GetComponent<GoalController>()) {
                Destroy(collision.transform.parent.gameObject);

                GameManager.singleton.AddScore(10);
                AudioManager.instance?.PlayPlatformDestructionSound();
                isSuperSpeedActive = false; 
            }
        } else {
            //Check if ball hits a death section
            DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
            if(deathPart){
                deathPart.HitDeathPart();
            }
                
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextColliison = true;
        Invoke("AllowCollision", 0.2f);

        perfectPass = 0;
        isSuperSpeedActive = false; 
    }

        // Update is called once per frame
    void Update()
    {
        if(perfectPass >= 3 && !isSuperSpeedActive) {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    private void AllowCollision(){
        ignoreNextColliison = false;
    }

    public void ResetBall()
    {
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        if(trail != null)
        {
            trail.enabled = false;
        }
        
        transform.position = startPos;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        if(trail != null)
        {
            trail.Clear();
            trail.enabled = true;
        }
    }
}
