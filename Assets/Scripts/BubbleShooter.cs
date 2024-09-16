using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    public GameObject BubbleToShoot;
    Arrow Arrow;
    private Layout Layout;

    //for bubble shooting
    public float shootStrength;
    public PhysicsMaterial2D BounceMaterial;
    bool readyToShoot;
    private CircleCollider2D col;
    
    [Header("Debugging")]
    public Transform bottomCollider;
    public Transform shootingPosition;

    //debugging
    //public TextMeshProUGUI debug1;

    //Bubble collision variables
        private HashSet<Bubble> visitedBubbles = new HashSet<Bubble>();


    void OnEnable()
    {
        Bubble.OnHasCollided += HasColided;    
    }


    void OnDisable()
    {
        Bubble.OnHasCollided -= HasColided;  
    }

    private void HasColided(Collision2D _collision)
    {
        print($"Has collided with: {_collision.transform.name}");
        //Getting hit info
            Rigidbody2D rb = _collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;                 //stopping velocity
            rb.angularVelocity = 0f;                    //stopping rotation
            rb.sharedMaterial = null;                   //Removing bouncy physics material
            rb.bodyType = RigidbodyType2D.Kinematic;
            Vector3 hitPos = rb.transform.position;
            GameObject shotBubble = rb.gameObject;
            //print("HIT: " + hitPos);

            //Putting Shot Bubble into the right place after collision
            //Round ROW position
            hitPos.y = Mathf.Round(hitPos.y);

            //IF EVEN row, apply offset
            if (hitPos.y % 2 == 0)
            {
                float hitPosX = hitPos.x;               //save original x value   
                float midValue = Mathf.Round(hitPos.x) + 0.05f;     //to which side of this value is hitPosX

                if (hitPosX >= midValue)                //Offset side - L or R
                {
                    hitPos.x = midValue + 0.5f;         //Bubble needs to go to the RIGHT OFFSET
                    print($"Right: {hitPos}");
                }
                else
                {
                    hitPos.x = midValue - 0.5f;         //Bubble needs to go to the LEFT OFFSET
                    print($"Left: {hitPos}");
                }   
            }
            else//ODD ROW
            {
                hitPos.x = Mathf.Round(hitPos.x);       //Round rowElement position
                print($"No offset needed: {hitPos}");
            }

            Vector3 correctBouncePosition = hitPos;
            shotBubble.transform.position = correctBouncePosition;

            
            //should it be here? - YES! Otherwise, the CheckForMatching will be called twice. Becouse the new ShotBall will colide with this and trigger this event again!
            shotBubble.transform.tag = "Bubble";


            //MATCHING BEGINS
            //After placing the bubble in correct position, check for matching bubbles

            //Clear visited bubbles before starting new Check
            visitedBubbles.Clear();
            
            //Start Checking
            checkForMatchingBubbles(shotBubble.GetComponent<Bubble>());

            FindAnyObjectByType<BubbleShooter>().SpawnShootingBubble();
        
        
    }

    private void Awake()
    {
        Arrow = FindObjectOfType<Arrow>();
        Layout = FindObjectOfType<Layout>();
        shootingPosition = transform.Find("BubblesPlaceholder");
        bottomCollider = Camera.main.transform.Find("Wall_D2");

        bottomCollider.gameObject.SetActive(false);
        col = BubbleToShoot.GetComponent<CircleCollider2D>();

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
        //debug1.text = "ReadyToShoot: " + readyToShoot;

        if (Input.GetMouseButtonDown(0))
        {
            shootBubble();
        }


    }

    public void SpawnShootingBubble() 
    {
        //IF empty then spawn Bubble
        if (shootingPosition.childCount == 0)
        {
            bottomCollider.gameObject.SetActive(false);
            GameObject go = Instantiate(bubblePrefab, shootingPosition.transform);
            Bubble bubble = go.GetComponent<Bubble>();
            bubble.SetBubbleColor((BubbleColor)Random.Range(0, 5));                    //Help from https://discussions.unity.com/t/using-random-range-to-pick-a-random-value-out-of-an-enum/119639
            Layout.BubbleNr++;
            //Layout.layout.Add(Layout.BubbleNr, bubble);

            go.transform.localPosition = Vector3.zero;
            BubbleToShoot = go.gameObject;
            BubbleToShoot.tag = "BubbleShot";
            BubbleToShoot.name = "BubbleToShoot" + Layout.BubbleNr;
            readyToShoot = true;
            turnOnArrow();

            Rigidbody2D rb = BubbleToShoot.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.sharedMaterial = BounceMaterial;                                         //bouncy material from https://discussions.unity.com/t/making-an-object-bounce-off-a-wall-the-same-way-light-bounces-off-of-a-mirror/94792/3

            col.radius = 0.5f;
        }

    }

    void shootBubble()
    {
        if (readyToShoot)
        {
            turnOffArrow();
            readyToShoot = false;
            // //Calculate which direction to shoot
            // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 dir = mousePos - transform.position;
            
            //Shoot the bubble
            Rigidbody2D rb = BubbleToShoot.GetComponent<Rigidbody2D>();
            rb.velocity = Arrow.GetDirection() * shootStrength;

            //unparent bubble
            BubbleToShoot.transform.SetParent(null);
            bottomCollider.gameObject.SetActive(true);
        }

        
    }


    void turnOnArrow()
    {
        Arrow.gameObject.SetActive(true);
    }

    void turnOffArrow()
    {
        Arrow.gameObject.SetActive(false);
    }

       //Bubble matching logic
    private void checkForMatchingBubbles(Bubble _shotBubble)             
    {
        List<Bubble> MatchingBubbles = new List<Bubble>();
        FindMatchingBubbles(_shotBubble, _shotBubble.BubbleColor, MatchingBubbles);
        
        //Found at least 3 matching bubbles, execute "Pop'ing" them
        if (MatchingBubbles.Count >= 3)
        {
            foreach (var bubble in MatchingBubbles)
            {
                
                PopBubble(bubble);
            }
        }
    }

    //Recursive fucntion to find all Matching Bubbles
    private void FindMatchingBubbles(Bubble bubble, BubbleColor bubbleColor, List<Bubble> matchingBubbles)
    {
        //if the bubble has already been visited, then return
        if (visitedBubbles.Contains(bubble)) return;

        //Mark current bubble as visited
        visitedBubbles.Add(bubble);
        matchingBubbles.Add(bubble);
        print("visitedBubbles: " + visitedBubbles.Count);

        //Scan for overlapping bubbles
        Collider2D[] OverlappingBubbles = Physics2D.OverlapCircleAll(bubble.transform.position, 0.8f);

        foreach (var overlappingCollider in OverlappingBubbles)
        {
            Bubble overlappingBubble = overlappingCollider.GetComponent<Bubble>();

            //if the overlapping bubble matches the color and hasn't been visited, add it to the list and continue recursion
            if (overlappingBubble != null && overlappingBubble.BubbleColor == bubbleColor && !visitedBubbles.Contains(overlappingBubble))
            {
                //matchingBubbles.Add(overlappingBubble);
                FindMatchingBubbles(overlappingBubble, bubbleColor, matchingBubbles);
            }
        }
    }


    //Check if bubble is "left hanging"
    //TODO: Fix logic
    private void isLeftHanging()
    {
        //If no overlapping Bubbles, only itself, then POP
        if (transform.CompareTag("Bubble"))             //to not affect Shooting Bubble
        {
            Collider2D[] OverlappingBubbles = Physics2D.OverlapCircleAll(transform.position, 0.8f);

            //Overlapping only self
            if (OverlappingBubbles.Length == 1)
            { 
                //PopBubble(this);
            } 
        }
    }

    //Bubble has been matched, "pop it"
    private void PopBubble(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
        //Add Score
        Destroy(bubble.gameObject);
    }
}
