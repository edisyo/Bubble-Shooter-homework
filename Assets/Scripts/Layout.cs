using System.Collections.Generic;
using UnityEngine;

public class Layout : MonoBehaviour
{
    [Header("Layout setup")]
    //Grid layout variables
    [SerializeField] private int maxRow = 5;
    [SerializeField] private int maxRowElements = 10;

    [SerializeField] private GameObject bubblePrefab;
   
    
    public Dictionary<Vector2, Bubble> layout = new Dictionary<Vector2, Bubble>();

    [SerializeField] private FloatVariable offset;

    [Header("Debugging")] 
    [SerializeField] private int bubbleNr = 0;


    //GETTERS and SETTERS
    public int BubbleNr
    {   
        get{return bubbleNr;}
        set{bubbleNr = value;}
    }
    

    // Start is called before the first frame update
    void Start()
    {
        CreateLayout();
        print("Dictionary: " + layout.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //

    }

    private void OnValidate() 
    {
        
    }

    private void CreateLayout()
    {
        //offset = bubblePrefab.transform.localScale.x / 2;

        //GRID
        for (int row = 1; row <= maxRow; row++)
        {
            //ROW LOGIC
            if (row % 2 == 0)
            {
                //ROW to the RIGHT
                for (int rowElement = 1; rowElement <= maxRowElements; rowElement++)
                {
                    //[ ] TODO: Create a function for this IF statement
                    if (bubblePrefab != null)
                    {
                        GameObject go = Instantiate(bubblePrefab, new Vector3(rowElement + offset.value, -row, 0), Quaternion.identity);
                        Bubble bubble = go.GetComponent<Bubble>();
                        bubble.SetRowAndColumn(row, rowElement);
                        bubble.SetBubbleColor((BubbleColor)Random.Range(0, 5));                     //Help from https://discussions.unity.com/t/using-random-range-to-pick-a-random-value-out-of-an-enum/119639
                        bubble.name = "Bubble"+BubbleNr;
                        BubbleNr++;

                        //test add to Dictionary
                        layout.Add(new Vector2(row,rowElement), bubble);
                    }
                    else { Debug.LogError("Bubble prefab missing"); }
                }
            }
            else
            {
                //ROW to the LEFT
                for (int rowElement = 1; rowElement <= maxRowElements; rowElement++)
                {
                    if (bubblePrefab != null)
                    {
                        GameObject go = Instantiate(bubblePrefab, new Vector3(rowElement, -row, 0), Quaternion.identity);
                        Bubble bubble = go.GetComponent<Bubble>();
                        bubble.SetRowAndColumn(row, rowElement);
                        bubble.SetBubbleColor((BubbleColor)Random.Range(0, 5));                     //Help from https://discussions.unity.com/t/using-random-range-to-pick-a-random-value-out-of-an-enum/119639
                        bubble.name = "Bubble" + BubbleNr;
                        BubbleNr++;

                        //test add to Dictionary
                        layout.Add(new Vector2(row,rowElement), bubble);

                    }
                    else { Debug.LogError("Bubble prefab missing"); }
                }
            }

        }
    }

    public void Debugging()
    {
        foreach (var item in layout)
        {
            print($"Dictionary:{item.Key} : {item.Value}");
        }
    }
}
