using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSprite : MonoBehaviour
{
    public Sprite cardUp;

    private ThreeOfSpades threeOfSpades;
    private SpriteRenderer spriteRenderer;
    private CanSelect canSelect;
    private UserInput userInput;

    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = ThreeOfSpades.GenerateDeck();
        threeOfSpades = FindObjectOfType<ThreeOfSpades>();
        userInput = FindObjectOfType<UserInput>();
        int i = 0;
        foreach(string card in deck)
        {
            if(this.name == card)
            {
                cardUp = threeOfSpades.cardFaces[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        canSelect = GetComponent<CanSelect>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = cardUp;
        if(userInput.slot1)
        {
            if (userInput.slot1.name == name)
            {
                spriteRenderer.color = Color.yellow;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
        if(userInput.nextChance)
        {
            //print("NC " + userInput.nextChance);
            if(!canSelect.isRaised && canSelect.selectable)
            {
                //print(transform.name + "  " + transform.position);
                //print("New pos" + "  " + transform.position.x + " " + (transform.position.y + 0.03f) + " " + transform.position.z);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                canSelect.isRaised = true;
                //canSelect.selectable = false;
            }
        }
    }
}
