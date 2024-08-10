using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    Transform shootingPosition;
    public GameObject BubbleToShoot;
    GameObject Arrow;

    //for bubble shooting
    bool readyToShoot;
    public float shootStrength;

    //debugging
    public TextMeshProUGUI debug1;


    private void Awake()
    {
        Arrow = transform.Find("Arrow").gameObject;
        shootingPosition = transform.Find("BubblesPlaceholder");
    }

    // Start is called before the first frame update
    void Start()
    {
        turnOffArrow();
        readyToShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBubble();
        turnOnArrow();

        debug1.text = "ReadyToShoot: " + readyToShoot;

        if (Input.GetMouseButtonDown(0))
        {
            print("mouse L pressed");
            shootBubble();
        }


    }

    void SpawnBubble() 
    {
        //IF empty then spawn Bubble
        if (shootingPosition.childCount == 0)
        {
            print("spawning");
            GameObject go = Instantiate(bubblePrefab, shootingPosition.transform);
            go.transform.localPosition = Vector3.zero;
            BubbleToShoot = go.gameObject;
            readyToShoot = true;
        }

    }

    void shootBubble()
    {
        if (readyToShoot)
        {
            readyToShoot = false;
            print("shooting");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector2 dir = mousePos - transform.position;
            BubbleToShoot.GetComponent<Rigidbody2D>().velocity = dir * shootStrength;
            //SpawnBubble();
        }

        
    }
    void turnOnArrow()
    {
        Arrow.SetActive(true);
    }

    void turnOffArrow()
    {
        Arrow.SetActive(false);
    }
}
