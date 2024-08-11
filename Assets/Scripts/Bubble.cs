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
            rb.bodyType = RigidbodyType2D.Static;
            Vector3 hitPos = rb.transform.localPosition;
            GameObject go = rb.gameObject;
            //print("HIT: " + hitPos);

            //Putting Shot Bubble into the right place after collision
            
            hitPos.y = Mathf.Round(hitPos.y);           //Round ROW position

            if (hitPos.y % 2 == 0)                      //IF EVEN row, apply offset
            {
                //print("EVEN ROW");
                float hitPosX = hitPos.x;               //save original x value   
                float midValue = Mathf.Round(hitPos.x) + 0.05f;     //to which side of this value is hitPosX

                //print(hitPosX + " >= "+ midValue);
                if (hitPosX >= midValue)                //Offset side - L or R
                {
                    
                    hitPos.x = midValue + 0.5f;         //Bubble needs to go to the RIGHT OFFSET
                    //print("Offset to RIGHT");
                }
                else
                {
                    hitPos.x = midValue - 0.5f;         //Bubble needs to go to the LEFT OFFSET
                    //print("Offset to LEFT");
                }
                
                go.transform.position = hitPos;         //Apply bounce position
                
            }
            else
            {
                //print("ODD ROW");
                hitPos.x = Mathf.Round(hitPos.x);       //Round rowElement position
                go.transform.position = hitPos;         //Apply bounce position
            }

            go.transform.tag = "Bubble";
            go.transform.name = "ShotBubble";
            FindAnyObjectByType<BubbleShooter>().SpawnShootingBubble();
            
            //CalculatePoints();
        }
    }

    public BubbleColor GetBubbleColor()                                 //GET Bubbles Color
    {
        print("Bubble Color is: " + bubbleColor);
        return bubbleColor;
    }

    public void SetBubbleColor(BubbleColor _bubbleColor)                //SET Bubbles Color

    {                                                                   //with a little bit of help from https://stackoverflow.com/questions/22335103/c-sharp-how-to-use-get-set-and-use-enums-in-a-class and https://www.youtube.com/watch?v=HzIqrlSbjjU&list=PLX2vGYjWbI0S8YpPPKKvXZayCjkKj4bUP&index=3
        //SpriteRenderer spriteColor = GetComponent<SpriteRenderer>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sharedMaterial.color = Color.white;                      //for safety to not get weird tints
        bubbleColor = _bubbleColor;                                     //For getting the color
        print("Changing Bubble Color to: " + _bubbleColor);
        switch (_bubbleColor)
        {
            case BubbleColor.Yellow:
                sprite.color = c_yellow;
                print("Changing to YELLOW");
                print("Color: " + Color.yellow);
                break;
            case BubbleColor.Blue:
                sprite.color = c_blue;
                print("Changing to BLUE");
                print("Color: " + GetComponent<SpriteRenderer>().color);
                break;
            case BubbleColor.Pink:
                sprite.color = c_pink;
                print("Changing to PINK");
                print("Color: " + GetComponent<SpriteRenderer>().color);
                break;
            case BubbleColor.Cyan:
                sprite.color = c_cyan;
                print("Changing to CYAN");
                print("Color: " + GetComponent<SpriteRenderer>().color);
                break;
            case BubbleColor.Green:
                sprite.color = c_green;
                print("Changing to GREEN");
                print("Color: " + GetComponent<SpriteRenderer>().color);
                break;
            case BubbleColor.Red:
                sprite.color = c_red;
                print("Changing to RED");
                print("Color: " + GetComponent<SpriteRenderer>().color);
                break;
            default:
                break;
        }
    }

}
