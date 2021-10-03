using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject player;
    public GameObject textbox;
    public GameObject text;

    private SpriteRenderer sprite;
    private TextMesh words;
    private bool talking = false;
    private float timeTalked;
    private float maxTalkTime = 2.5F;

    // Start is called before the first frame update
    void Start()
    {
        sprite = textbox.GetComponent<SpriteRenderer>();
        words = text.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && (Input.GetKeyDown(KeyCode.E) || timeTalked + maxTalkTime < Time.time || Vector3.Distance(transform.position, player.transform.position) > 5))
        {
            talking = false;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
            words.color = new Color(words.color.r, words.color.g, words.color.b, 0);
        }
        else if (!talking && Vector3.Distance(transform.position, player.transform.position) < 3 && Input.GetKeyDown(KeyCode.E))
        {
            talking = true;
            timeTalked = Time.time;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
            words.color = new Color(words.color.r, words.color.g, words.color.b, 1);
        }
    }
}
