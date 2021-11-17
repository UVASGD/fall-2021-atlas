using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public List<string> menuOptions;
    public List<string> actions;
    public GameObject dummyString;
    public GameObject cursor;

    private SpriteRenderer sp;
    private Transform tf;
    private List<Renderer> listConts;
    private Vector3 location;
    private float height;
    private float width;
    private int size;
    private int pos;
    private bool hidden = true;
    private bool growingH;
    private bool growingW;

    // Start is called before the first frame update
    void Start()
    {
        // Initiate listConts List
        listConts = new List<Renderer>();

        // Gets info on the menu components
        sp = GetComponent<SpriteRenderer>();
        tf = transform;
        location = new Vector3(tf.position.x, tf.position.y, tf.position.z);
        height = sp.size.y;
        width = sp.size.x;
        tf.position = new Vector3(location.x - (width - 0.5F)/2, location.y + (height -0.5F)/2, location.z);
        sp.size = new Vector2(0, 0);


        // Sets up cursor position
        cursor.transform.position = new Vector3(tf.position.x - (width * 0.35F), tf.position.y + height * 0.3F, 0);
        listConts.Add(cursor.GetComponent<Renderer>());
        listConts[0].material.color = new Color(255, 255, 255, 0);

        // Sets up options of menu and spacing
        size = menuOptions.Count;
        for (int i = 0; i < size; i++)
        {
            GameObject str = Instantiate(dummyString, new Vector3(tf.position.x - (width * 0.3F), tf.position.y + (height * 0.3F) - (height * 0.6F) / (size - 1) * i, 0), Quaternion.identity);
            str.transform.parent = tf;
            Renderer ren = str.GetComponent<MeshRenderer>();
            ren.sortingLayerName = "UI";
            ren.sortingOrder = 2;
            TextMesh txt = str.GetComponent<TextMesh>();
            txt.text = menuOptions[i];
            listConts.Add(ren);
            listConts[i+1].material.color = new Color(255, 255, 255, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hidden && Input.anyKey && !growingW && !growingH)
        {
            growingH = true;
            sp.size = new Vector2(0.5F, 0.5F);
        }

        if(growingH && sp.size.y < height)
        {
            sp.size = new Vector2(0.5F, (height - 0.5F) / 15 + sp.size.y);
            tf.position = new Vector3(tf.position.x, location.y + (height - 0.5F) / 2 - (sp.size.y - 0.5F) / 2, location.z);
            if (sp.size.y >= height)
            {
                sp.size = new Vector2(0.5F, height);
                tf.position = new Vector3(tf.position.x, location.y, location.z);
                growingW = true;
            }
        }

        if (growingW && sp.size.x < width)
        {
            sp.size = new Vector2((width + 0.5F) / 15 + sp.size.x, height);
            tf.position = new Vector3(location.x - (width - 0.5F) / 2 + (sp.size.x - 0.5F) / 2, location.y, location.z);
            if (sp.size.x >= width)
            {
                sp.size = new Vector2(width, height);
                tf.position = new Vector3(location.x, location.y, location.z);
                foreach(Renderer ren in listConts)
                {
                    ren.material.color = new Color(255, 255, 255, 1);
                }
                hidden = false;
            }
        }

        if (!hidden)
        {
            // Updates cursor positions based off of input
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if (pos >= size - 1)
                    pos = -1;
                pos++;
                cursor.transform.position = new Vector3(tf.position.x - (width * 0.35F), tf.position.y + height * 0.3F - (height * 0.6F) / (size - 1) * pos, 0);
            }
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                if (pos <= 0)
                    pos = size;
                pos--;
                cursor.transform.position = new Vector3(tf.position.x - (width * 0.35F), tf.position.y + height * 0.3F - (height * 0.6F) / (size - 1) * pos, 0);
            }

            // Activates based off of entry value
            if (Input.GetKeyDown(KeyCode.Return))
            {
                takeAction();
            }
        }
    }

    public void takeAction()
    {
        // TODO
        
        /*
        switch (actions[pos][0]) {
            case 'L':
                SceneManager.LoadScene(actions[pos].Substring(1));
                break;
            case 'M':
                string[] ins = actions[pos].Split(' ');
                width = float.Parse(ins[1]);
                height = float.Parse(ins[2]);
                break;
        }
        */
    }
}
