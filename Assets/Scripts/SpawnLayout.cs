using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnLayout : MonoBehaviour
{


    public GameObject bubblePrefab;
    float offset;
    public int BubbleNr = 0;


    // Start is called before the first frame update
    void Start()
    {
        offset = bubblePrefab.transform.localScale.x / 2;

        //GRID
        for (int row = -1; row >= -5; row--)
        {
            //ROW LOGIC
            if (row % 2 == 0)
            {
                
                //ROW to the RIGHT
                for (int rowElement = 2; rowElement <= 10; rowElement++)
                {
                    if (bubblePrefab != null)
                    {
                        GameObject go = Instantiate(bubblePrefab, new Vector3(rowElement - offset, row, 0), Quaternion.identity);
                        Bubble bubble = go.GetComponent<Bubble>();
                        bubble.SetBubbleColor((BubbleColor)Random.Range(0, 5));                     //Help from https://discussions.unity.com/t/using-random-range-to-pick-a-random-value-out-of-an-enum/119639
                        bubble.name = "Bubble"+BubbleNr;
                        BubbleNr++;
                    }
                    else { Debug.LogError("Bubble prefab missing"); }
                }
            }
            else
            {
                //ROW to the LEFT
                for (int rowElement = 1; rowElement <= 9; rowElement++)
                {
                    if (bubblePrefab != null)
                    {
                        GameObject go = Instantiate(bubblePrefab, new Vector3(rowElement, row, 0), Quaternion.identity);
                        Bubble bubble = go.GetComponent<Bubble>();
                        bubble.SetBubbleColor((BubbleColor)Random.Range(0, 5));                     //Help from https://discussions.unity.com/t/using-random-range-to-pick-a-random-value-out-of-an-enum/119639
                        bubble.name = "Bubble" + BubbleNr;
                        BubbleNr++;

                    }
                    else { Debug.LogError("Bubble prefab missing"); }
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
