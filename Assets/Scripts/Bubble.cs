using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    

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
            rb.velocity = Vector2.zero; //stopping velocity
            rb.angularVelocity = 0f; //stopping rotation
            rb.sharedMaterial = null; //Removing bouncy physics material
            rb.bodyType = RigidbodyType2D.Static;
            Vector3 hitPos = rb.transform.localPosition;
            GameObject go = rb.gameObject;
            print("HIT: " + hitPos);

            //Putting Shot Bubble into the right place after collision
            
            hitPos.y = Mathf.Round(hitPos.y);

            if (hitPos.y % 2 == 0)                  //IF EVEN row, apply offset
            {
                print("EVEN ROW");
                float hitPosX = hitPos.x;           //save original x value   
                float midValue = Mathf.Round(hitPos.x) + 0.05f;     //to which side of this value is hitPosX

                print(hitPosX + " >= "+ midValue);
                if (hitPosX >= midValue)            //Offset side - L or R
                {
                    
                    hitPos.x = midValue + 0.5f;     //Bubble needs to go to the LEFT OFFSET
                    print("Offset to LEFT");
                }
                else
                {
                    hitPos.x = midValue - 0.5f;     //Bubble needs to go to the RIGHT OFFSET
                    print("Offset to RIGHT");
                }
                
                //hitPos.y = Mathf.Round(hitPos.y);   //Round Row position
                go.transform.position = hitPos;     //Apply bounce position
                
            }
            else
            {
                print("ODD ROW");
                hitPos.x = Mathf.Round(hitPos.x);
                //hitPos.y = Mathf.Round(hitPos.y);
                go.transform.position = hitPos;     //Apply bounce position
            }

            go.transform.tag = "Bubble";
            go.transform.name = "ShotBubble";
            FindAnyObjectByType<BubbleShooter>().SpawnShootingBubble();

            //CalculatePoints();
        }
    }
}
