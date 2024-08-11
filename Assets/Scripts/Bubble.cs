using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Bubble;


public enum BubbleColor { Yellow, Blue, Pink, Cyan, Green, Red };       //Needs to be outside of class, otherwise can access it only through Bubble.BubbleColor.x not BubbleColor.x

public class Bubble : MonoBehaviour
{
    //private Color color;
    [SerializeField] private BubbleColor bubbleColor;
    Color c_yellow = new Color(0.97f, 0.9f, 0, 1f);
    Color c_blue = new Color(0, 0.39f, 0.97f, 1f);
    Color c_pink = new Color(0.97f, 0, 0.57f, 1f);
    Color c_cyan = new Color(0, 0.97f, 0.95f, 1f);
    Color c_green = new Color(0, 0.9f, 0, 1f);
    Color c_red = new Color(0.9f, 0.15f, 0, 1f);


    //List<Collider2D> OverlappingBubbles = new List<Collider2D>();
    private HashSet<Bubble> visitedBubbles = new HashSet<Bubble>();


    //Called when script instance is being loaded
    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BubbleShot"))
        {
            //Getting hit info
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
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
                }
                else
                {
                    hitPos.x = midValue - 0.5f;         //Bubble needs to go to the LEFT OFFSET
                }   
            }
            else//ODD ROW
            {
                hitPos.x = Mathf.Round(hitPos.x);       //Round rowElement position
            }

            Vector3 correctBouncePosition = hitPos;
            shotBubble.transform.position = correctBouncePosition;

            
            //should it be here - YES! Otherwise, the CheckForMatching will be called twice. Becouse the new ShotBall will colide with this and trigger this event again!
            shotBubble.transform.tag = "Bubble";


            //MATCHING BEGINS
            //After placing the bubble in correct position, check for matching bubbles

            //Clear visited bubbles before starting new Check
            visitedBubbles.Clear();
            
            //Start Checking
            checkForMatchingBubbles(shotBubble.GetComponent<Bubble>());

            FindAnyObjectByType<BubbleShooter>().SpawnShootingBubble();
        }
    }

    //My Bubble Matching logic. Couldnt figure out how to make it recursive, checked only first overlapping bubbles
    private void CheckForOverlappingBubbles (Bubble BubbleToCheckFrom)                  //FIRST it will be the just Shot Bubble
    {
        Collider2D[] OtherBubbles = Physics2D.OverlapCircleAll(BubbleToCheckFrom.transform.position, 0.8f);        //Gets access to the surrounding bubbles

        print("Checking surrounding bubbles for - " + BubbleToCheckFrom.name + " - at: " + BubbleToCheckFrom.transform.position);

        //Checks surrounding bubbles and puts them into a LIST
        foreach (Collider2D otherCollider in OtherBubbles)                  
        {
            //Debug.Log("Overlapping with: " + otherCollider.gameObject.name, otherCollider);
            
            //Make sure to not add the JUST shotBubble to the overlapping bubble list
            if (otherCollider.name != BubbleToCheckFrom.name)               
            {
                if (BubbleToCheckFrom.bubbleColor == otherCollider.GetComponent<Bubble>().bubbleColor)
                {
                    ///OverlappingBubbles.Add(otherCollider);                  //All overlapping bubbles with the same color
                    //CheckForOverlappingBubbles(BubbleToCheckFrom);           //Unity crashes (: 
                }
            }
        }
    }

    //Bubble matching logic
    private void checkForMatchingBubbles(Bubble _shotBubble)             
    {
        List<Bubble> MatchingBubbles = new List<Bubble>();
        FindMatchingBubbles(_shotBubble, _shotBubble.bubbleColor, MatchingBubbles);

        foreach (var bubble in MatchingBubbles)
        {
            print("Matched bubbles: " + bubble.name);
        }
        

        if (MatchingBubbles.Count >= 3)
        {
            foreach (var bubble in MatchingBubbles)
            {
                
                PopBubble(bubble);
            }
        }
    }

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
            if (overlappingBubble != null && overlappingBubble.bubbleColor == bubbleColor && !visitedBubbles.Contains(overlappingBubble))
            {
                //matchingBubbles.Add(overlappingBubble);
                FindMatchingBubbles(overlappingBubble, bubbleColor, matchingBubbles);
            }
        }

    }

    private void PopBubble(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
    }

    //Get Bubbles BubbleColor
    public BubbleColor GetBubbleColor()                                 
    {
        return bubbleColor;
    }

    public void SetBubbleColor (BubbleColor _bubbleColor)                //SET Bubbles Color

    {                                                                   //with a little bit of help from https://stackoverflow.com/questions/22335103/c-sharp-how-to-use-get-set-and-use-enums-in-a-class and https://www.youtube.com/watch?v=HzIqrlSbjjU&list=PLX2vGYjWbI0S8YpPPKKvXZayCjkKj4bUP&index=3
        //SpriteRenderer spriteColor = GetComponent<SpriteRenderer>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sharedMaterial.color = Color.white;                      //for safety to not get weird tints
        bubbleColor = _bubbleColor;                                     //For getting the color
        //print("Changing Bubble Color to: " + _bubbleColor);
        switch (_bubbleColor)
        {
            case BubbleColor.Yellow:
                sprite.color = c_yellow;;
                break;
            case BubbleColor.Blue:
                sprite.color = c_blue;
                break;
            case BubbleColor.Pink:
                sprite.color = c_pink;
                break;
            case BubbleColor.Cyan:
                sprite.color = c_cyan;
                break;
            case BubbleColor.Green:
                sprite.color = c_green;
                break;
            case BubbleColor.Red:
                sprite.color = c_red;
                //print("Changing to RED");
                //print("Color: " + GetComponent<SpriteRenderer>().color);
                break;
            default:
                break;
        }
    }

}
