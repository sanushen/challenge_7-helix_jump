using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();
    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();

    void Awake() {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        LoadStage(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            Vector2 curTapPos = Input.mousePosition;

            if(lastTapPos == Vector2.zero) {
                lastTapPos = curTapPos;
            }

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if(Input.GetMouseButtonUp(0)){
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber){
        ClearLevels();
        
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];
        if (stage == null){
            Debug.Log("No stage found");
            return;
        }
        
        //Stage background color
        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
        
        //Ball Color
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;
        
        //Reset helix rotation
        transform.localEulerAngles = startRotation;
        
        //Create new level
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;
        
        for (int i = 0; i < stage.levels.Count; i++){
            spawnPosY -= levelDistance;
            
            //Create level within scene
            GameObject level = Instantiate(helixLevelPrefab, transform);
            Debug.Log("Level Spawned");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);
            
            // FIRST: Enable all parts to ensure we start with a clean slate
            foreach (Transform child in level.transform) {
                child.gameObject.SetActive(true);
            }
            
            //Disable sections
            int partsToDisable = 12 - stage.levels[i].partCount;
            
            // Create list of ALL parts first
            List<GameObject> allParts = new List<GameObject>();
            foreach (Transform t in level.transform) {
                allParts.Add(t.gameObject);
            }
            
            // Shuffle and disable the first X parts
            for (int j = 0; j < partsToDisable && j < allParts.Count; j++) {
                int randomIndex = Random.Range(j, allParts.Count);
                // Swap
                GameObject temp = allParts[j];
                allParts[j] = allParts[randomIndex];
                allParts[randomIndex] = temp;
                // Disable
                allParts[j].SetActive(false);
            }
            
            //List of active parts
            List<GameObject> leftParts = new List<GameObject>();
            foreach (Transform t in level.transform) {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                if(t.gameObject.activeInHierarchy) {
                    leftParts.Add(t.gameObject);
                }
            }
            
            //Death parts
            List<GameObject> deathParts = new List<GameObject>();
            while(deathParts.Count < stage.levels[i].deathPartCount && deathParts.Count < leftParts.Count){
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];
                if (!deathParts.Contains(randomPart)) {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }
        }
    }

    //Added this method because successive level restarts loads incorrect sections and death parts
    public void ClearLevels() {
        foreach (GameObject level in spawnedLevels) {
            Destroy(level);
        }
        spawnedLevels.Clear();
    }
}
