using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class BubbleShooter : MonoBehaviour
{
    //Necessary setup for Shooter
    public GameObject bubblePrefab;
    private BubbleToShoot bubbleToShoot;
    Arrow Arrow;
    private Layout Layout;

    //for bubble shooting
    public FloatVariable shootStrength;
    public PhysicsMaterial2D BounceMaterial;
    bool readyToShoot;

    //for bubble placement
    [SerializeField] private FloatVariable offset;
    
    [Header("Debugging")]
    public Transform bottomCollider;
    public Transform shootingPosition;

    //debugging

    //Bubble collision variables
    private HashSet<Bubble> visitedBubbles = new HashSet<Bubble>();
    private HashSet<Bubble> floatingVisitedBubbles = new HashSet<Bubble>();


    private void Awake()
    {
        Arrow = FindObjectOfType<Arrow>();
        Layout = FindObjectOfType<Layout>();
        shootingPosition = transform.Find("BubblesPlaceholder");
        bottomCollider = Camera.main.transform.Find("Wall_D2");

        bottomCollider.gameObject.SetActive(false);
    }

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

    void OnEnable()
    {
        BubbleToShoot.OnHasCollided += HasColided;    
    }


    void OnDisable()
    {
        BubbleToShoot.OnHasCollided -= HasColided;  
    }

    //Handles the logic for Bubble Collision and putting Bubble in its correct position
    private void HasColided(Collision2D _hitBubble, BubbleToShoot _shotBubble)
    {
        print($"<color=#67eb34>{_shotBubble.name}</color> has collided with: <color=#eb3434>{_hitBubble.transform.name}</color>");
        //Stop Shot Bubble
            Rigidbody2D rb = _shotBubble.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;                 //stopping velocity
            rb.angularVelocity = 0f;                    //stopping rotation
            rb.sharedMaterial = null;                   //Removing bouncy physics material
            rb.bodyType = RigidbodyType2D.Kinematic;

            //Switch Components - from BubbleToShoot to Bubble

            //BUG: Sometimes adds two Bubble scripts
            BubbleColor bc = _shotBubble.BubbleColor;
            _shotBubble.AddComponent<Bubble>();
            var bubbleToAttach = _shotBubble.GetComponent<Bubble>();
            bubbleToAttach.BubbleColor = bc;
            Destroy(_shotBubble);

            //Calculate attachment position
            bubbleToAttach.transform.position = AttachShotBubble(_hitBubble, bubbleToAttach);

            //should it be here? - YES! Otherwise, the CheckForMatching will be called twice.
            bubbleToAttach.transform.tag = "Bubble";       

            //* MATCHING BEGINS
            //After placing the bubble in correct position, check for matching bubbles

            //Clear visited bubbles before starting new Check
            visitedBubbles.Clear();
            
            //Start Checking
            checkForMatchingBubbles(bubbleToAttach);
    }

    Vector3 AttachShotBubble(Collision2D _hitBubble, Bubble _bubbleToAttach)
    {
        var _hitPos = _hitBubble.GetContact(0).point;
        var hitBubble = _hitBubble.gameObject.GetComponent<Bubble>();
        var attachPos = _hitPos;

        //ATTACH TO EVEN ROW
        //Take 1/5 of the bubble's scale for THRESHOLD
        if(_hitPos.y <= _hitBubble.transform.position.y - 0.2f){
            attachPos.y = _hitBubble.transform.position.y - 1;
            _bubbleToAttach.Row = hitBubble.Row + 1;
            if(_hitPos.x >= _hitBubble.transform.position.x)                
            {
                //BELOW RIGHT
                
                attachPos.x = _hitBubble.transform.position.x + offset.value;

                //Check to see if hit EVEN ROW
                if(hitBubble.Row % 2 == 0)
                {
                    //hit even
                    _bubbleToAttach.Column = hitBubble.Column + 1;
                }else
                {
                     //hit uneven
                    _bubbleToAttach.Column = hitBubble.Column;
                }

                
                print($"BELOW RIGHT : {attachPos}");
            }else
            {
                //BELOW LEFT
                attachPos.x = _hitBubble.transform.position.x - offset.value;

                //Check to see if hit EVEN ROW
                if(hitBubble.Row % 2 == 0)
                {
                    print("hit EVEN row");
                    _bubbleToAttach.Column = hitBubble.Column;
                }else
                {
                    print("hit UNEVEN row");
                    _bubbleToAttach.Column = hitBubble.Column -1;
                }

                print($"BELOW LEFT: {attachPos}");
            }
        }else{
            //ATTACH TO SAME ROW
            attachPos.y = _hitBubble.transform.position.y;
            _bubbleToAttach.Row = hitBubble.Row;
            
            if(_hitPos.x >= _hitBubble.transform.position.x)                
            {
                attachPos.x = _hitBubble.transform.position.x + 1;
                _bubbleToAttach.Column = hitBubble.Column + 1;
                print($"SAME RIGHT : {attachPos}");
            }else
            {
                attachPos.x = _hitBubble.transform.position.x - 1;
                _bubbleToAttach.Column = hitBubble.Column - 1;
                print($"SAME LEFT: {attachPos}");
            }
        }
        return attachPos;
    }

    public void SpawnShootingBubble() 
    {
        //IF empty then spawn Bubble
        if (shootingPosition.childCount == 0)
        {
            bottomCollider.gameObject.SetActive(false);
            var go = Instantiate(bubblePrefab, shootingPosition.transform);

            //SWITCH bubble scripts
            Destroy(go.GetComponent<Bubble>());
            go.AddComponent<BubbleToShoot>();

            //SET BubbleToShootColor
            bubbleToShoot = go.GetComponent<BubbleToShoot>();
            bubbleToShoot.SetBubbleColor((BubbleColor)Random.Range(0, 5));                    //Help from https://discussions.unity.com/t/using-random-range-to-pick-a-random-value-out-of-an-enum/119639

            //Change TAG
            bubbleToShoot.tag = "BubbleShot";
            Layout.BubbleNr++;
            bubbleToShoot.name = "BubbleToShoot" + Layout.BubbleNr;

            Rigidbody2D rb = bubbleToShoot.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.sharedMaterial = BounceMaterial;                                         //bouncy material from https://discussions.unity.com/t/making-an-object-bounce-off-a-wall-the-same-way-light-bounces-off-of-a-mirror/94792/3

            readyToShoot = true;
            turnOnArrow();
        }

    }
    void shootBubble()
    {
        if (readyToShoot)
        {
            turnOffArrow();
            readyToShoot = false;
            
            //Shoot the bubble
            Rigidbody2D rb = bubbleToShoot.GetComponent<Rigidbody2D>();
            rb.velocity = Arrow.GetDirection() * shootStrength.value;

            //Unparent bubble
            bubbleToShoot.transform.SetParent(null);
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
            //[ ]FIXME: First must FIX Shooting Bubble Row and Column assigning 
            //[ ]TODO: Check for leftover Bubbles. Do 1 more function similiar to FindMatchingBubbles and put those Bubbles
            //      in a list. If 1 Bubbles in list is in  1 ROW, then dont pop this list, else POP



            foreach (var bubble in MatchingBubbles)
            {
                PopBubble(bubble);
            }
        }

        //Spawn next Bubble, this is the "end of the turn"
        SpawnShootingBubble();
    }


    //Recursive fucntion to find all Matching Bubbles
    private void FindMatchingBubbles(Bubble bubble, BubbleColor bubbleColor, List<Bubble> matchingBubbles)
    {
        //if the bubble has already been visited, then return
        if (visitedBubbles.Contains(bubble)) return;

        //Mark current bubble as visited
        visitedBubbles.Add(bubble);
        matchingBubbles.Add(bubble);

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

    //FIXME: Recursive function to find all LEFTOVER FLOATING Bubbles after a Bubble Matching has been done
    //      Check for leftover Bubbles. Do 1 more function similiar to FindMatchingBubbles and put those Bubbles
    //      in a list. If 1 Bubbles in list is in  1 ROW, then dont pop this list, else POP
    private void CheckLeftoverBubbles(Bubble bubble)
    {
        //Peform BFS or DFS from all bubble in the First Row

        //Mark Visited

        //Check in all Bubble list if visited, if not - floating

        //Floating bubbles.Pop
    }

    //Bubble has been matched, "pop it"
    private void PopBubble(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
        //Add Score
        //TODO: Remove Bubble from the Layout list!
        Destroy(bubble.gameObject);

    }
}
