using System;
using System.Collections.Generic;
using UnityEngine;


public enum BubbleColor { Yellow, Blue, Pink, Cyan, Green, Red };       //Needs to be outside of class, otherwise can access it only through Bubble.BubbleColor.x not BubbleColor.x

public class Bubble : MonoBehaviour
{
    //private Color color;
    [SerializeField] private BubbleColor bubbleColor;
    public BubbleColor BubbleColor 
    {
        get{ return bubbleColor;}
    }

    Color c_yellow = new Color(0.97f, 0.9f, 0, 1f);
    Color c_blue = new Color(0, 0.39f, 0.97f, 1f);
    Color c_pink = new Color(0.97f, 0, 0.57f, 1f);
    Color c_cyan = new Color(0, 0.97f, 0.95f, 1f);
    Color c_green = new Color(0, 0.9f, 0, 1f);
    Color c_red = new Color(0.9f, 0.15f, 0, 1f);

  
    [SerializeField] private int column = 0;
    [SerializeField] private int row = 0;




    public static Action<Collision2D> OnHasCollided;

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BubbleShot"))
        {
            //Calculate collision logic inside BubbleShooter
            OnHasCollided?.Invoke(collision);
        }
    }

 

    //GET Bubble's BubbleColor
    public BubbleColor GetBubbleColor()                                 
    {
        return bubbleColor;
    }

    public void SetRowAndColumn(int _row, int _column)
    {
        row = _row;
        column = _column;
    }

    //SET Bubble's  BubbleColor
    public void SetBubbleColor (BubbleColor _bubbleColor)                
    {                                                                   //with a little bit of help from https://stackoverflow.com/questions/22335103/c-sharp-how-to-use-get-set-and-use-enums-in-a-class and https://www.youtube.com/watch?v=HzIqrlSbjjU&list=PLX2vGYjWbI0S8YpPPKKvXZayCjkKj4bUP&index=3
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sharedMaterial.color = Color.white;                      //for safety to not get weird tints

        //Set the global parameter
        bubbleColor = _bubbleColor;                                     

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
                break;
            default:
                break;
        }
    }

}
