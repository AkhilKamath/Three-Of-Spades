using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    public bool nextChance = false;

    // Start is called before the first frame update
    void Start()
    {
        slot1 = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit)
            {
                if(hit.collider.CompareTag("Card"))
                {
                    //clicked card
                    Card(hit.collider.gameObject);
                }
                else if(hit.collider.CompareTag("Button"))
                {
                    Button();
                }
            }
        }
    }

    void Card(GameObject cardObj)
    {
        //print("Card selected is " + cardObj.name);
        if (slot1 = this.gameObject)
        {
            slot1 = cardObj;
        }
    }

    void Button()
    {
        
        System.Random rnd = new System.Random();
        int next = rnd.Next(0, 4);
        ThreeOfSpades.current = ThreeOfSpades.suites[next];
        //print("Button selected, next suite is " + ThreeOfSpades.current);
        nextChance = true;
    }
}
