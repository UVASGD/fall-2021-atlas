using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject player;
    public GameObject textbox;
    public int DialogueLine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueScript.talking && Vector3.Distance(transform.position, player.transform.position) < 3 && Input.GetKeyDown(KeyCode.Return))
        {
            textbox.gameObject.SendMessage("readDialogue", DialogueLine, SendMessageOptions.RequireReceiver);
        }
    }
}
