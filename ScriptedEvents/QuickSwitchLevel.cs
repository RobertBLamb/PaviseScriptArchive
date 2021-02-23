using UnityEngine;
using System.Collections;

public class QuickSwitchLevel : MonoBehaviour {

    [System.Serializable]
    public struct level
    {
        public GameObject levelPrefab;
        public Sprite BGSprite;
        public Transform playerSpawn;
    };
    public level[] levels;

    public GameObject currentBGObj;
    public float offset;

    public GameObject player;
    public Vector2 tempPlayerVel;

    public SpriteRenderer BGRenderer;
    public int currentLvlNum;
    public int defaultLvlNum;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Need to plug in BG object
        //BGRenderer = GetComponent<SpriteRenderer>();
        currentLvlNum = defaultLvlNum;
        SetLvlAndBG(currentLvlNum);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("f"))
        {
            currentLvlNum++;
            if (currentLvlNum >= levels.Length)
            {
                currentLvlNum = 0;
            }
            SetLvlAndBG(currentLvlNum);
        }
    }

    void SetLvlAndBG (int levelNum)
    {
        if (currentBGObj != null)
        {
            player.transform.parent = null;
            GameObject.Destroy(currentBGObj);
            GameObject[] existingLevels = GameObject.FindGameObjectsWithTag("LevelSection");

            for (int i = 0; i < existingLevels.Length; i++)
            {
                GameObject.Destroy(existingLevels[i]);
            }
            GameObject[] existingPlats = GameObject.FindGameObjectsWithTag("Platform");

            for (int i = 0; i < existingPlats.Length; i++)
            {
                GameObject.Destroy(existingPlats[i]);
            }

        }
        tempPlayerVel = player.GetComponent<Rigidbody2D>().velocity;
        currentBGObj = (Instantiate(levels[currentLvlNum].levelPrefab, (new Vector3((player.transform.position.x + offset), 0)), new Quaternion(0, 0, 0, 0)) as GameObject);

        for (int i = 0; i < currentBGObj.transform.childCount; i++)
        {
            GameObject currentChild = currentBGObj.transform.GetChild(i).gameObject;
            if(currentChild.CompareTag("PlayerSpawn"))
                levels[levelNum].playerSpawn = currentChild.transform;
        }
        player.transform.position = new Vector3 (levels[levelNum].playerSpawn.position.x, levels[levelNum].playerSpawn.position.y);

        BGRenderer.sprite = levels[levelNum].BGSprite;
    }
}
