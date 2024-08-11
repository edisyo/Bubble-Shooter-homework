using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    Transform shootingPosition;
    public GameObject BubbleToShoot;
    GameObject Arrow;

    //for bubble shooting
    public float shootStrength;
    public PhysicsMaterial2D BounceMaterial;
    bool readyToShoot;
    private CircleCollider2D col;
    

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
        SpawnShootingBubble();
    }

    // Update is called once per frame
    void Update()
    {
        
        turnOnArrow();

        debug1.text = "ReadyToShoot: " + readyToShoot;

        if (Input.GetMouseButtonDown(0))
        {
            print("mouse L pressed");
            shootBubble();
        }


    }

    public void SpawnShootingBubble() 
    {
        //IF empty then spawn Bubble
        if (shootingPosition.childCount == 0)
        {
            print("spawning");
            GameObject go = Instantiate(bubblePrefab, shootingPosition.transform);
            //TODO: Randomize Bubble Color()
            go.transform.localPosition = Vector3.zero;
            BubbleToShoot = go.gameObject;
            BubbleToShoot.tag = "BubbleShot";
            BubbleToShoot.name = "BubbleToShoot";
            readyToShoot = true;

            Rigidbody2D rb = BubbleToShoot.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.sharedMaterial = BounceMaterial; //bouncy material from https://discussions.unity.com/t/making-an-object-bounce-off-a-wall-the-same-way-light-bounces-off-of-a-mirror/94792/3

            col = BubbleToShoot.GetComponent<CircleCollider2D>();
            col.radius = 0.5f;
            
        }

    }

    void shootBubble()
    {
        if (readyToShoot)
        {
            readyToShoot = false;
            print("shooting");
            //Calculate which direction to shoot
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - transform.position;
            
            //Shoot the bubble
            Rigidbody2D rb = BubbleToShoot.GetComponent<Rigidbody2D>();
            rb.velocity = dir * shootStrength;

            //unparent bubble
            BubbleToShoot.transform.SetParent(null);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
