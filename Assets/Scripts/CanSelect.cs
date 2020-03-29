using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSelect : MonoBehaviour
{
    public bool selectable = false;
    public bool isRaised = false;
    public string suite;
    public int value; //ranges from 0..12 for 2..K,A
    public int point; //either 0, 5, 10 or 30 based on rules of three of spades
    private string valueString; // Start is called before the first frame update
    void Start()
    {
        point = 0;
        if(CompareTag("Card"))
        {
            suite = transform.name[0].ToString();

            valueString = transform.name.Substring(1, transform.name.Length - 1);
            if(valueString == "A")
            {
                value = 12;
                point = 10;
            }
            if (valueString == "K")
            {
                value = 11;
                point = 10;
            }
            if (valueString == "Q")
            {
                value = 10;
                point = 10;
            }
            if (valueString == "J")
            {
                value = 9;
            }
            if (valueString == "10")
            {
                value = 8;
            }
            if (valueString == "9")
            {
                value = 7;
            }
            if (valueString == "8")
            {
                value = 6;
            }
            if (valueString == "7")
            {
                value = 5;
            }
            if (valueString == "6")
            {
                value = 4;
            }
            if (valueString == "5")
            {
                value = 3;
                point = 5;
            }
            if (valueString == "4")
            {
                value = 2;
            }
            if (valueString == "3")
            {
                value = 1;
                if (suite == "S")
                    point = 30;
            }
            if (valueString == "2")
            {
                value = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ThreeOfSpades.current == transform.name[0].ToString() || ThreeOfSpades.trump == transform.name[0].ToString())
        {
            selectable = true;
        }
        else
        {
            selectable = false;
        }
        if(isRaised && !selectable)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            isRaised = false;
        }
    }
}
