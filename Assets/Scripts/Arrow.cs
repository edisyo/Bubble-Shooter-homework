using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Vector3 mousePos;
    Vector3 Look;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
    }

    void LookAtMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Look = transform.InverseTransformPoint(mousePos); //Code from BudGames from youtube https://www.youtube.com/watch?v=1Oda2M4BoNs
        angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg - 90;
        
        transform.Rotate(0, 0, angle);
    }

    public Vector3 GetDirection()
    {
        return transform.up;
    }
}
