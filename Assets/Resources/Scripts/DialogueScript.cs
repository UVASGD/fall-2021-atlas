using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    public static bool talking;

    public TextMesh text;
    public TextMesh nametag;
    public SpriteRenderer pic1;
    public SpriteRenderer pic2;
    public GameObject nameBox;
    public int txtSpeed;

    private SpriteRenderer sp;
    private SpriteRenderer spName;
    private Transform tfName;
    private Transform tf;
    private float height;
    private float width;
    private int size;
    private int at;
    private bool growingH;
    private bool growingW;
    private string path;
    private string[] script;
    private string toRead;
    private int reader;
    private int readcnt;

    // Start is called before the first frame update
    void Start()
    {
        talking = false;

        // Name Box Components
        spName = nameBox.GetComponent<SpriteRenderer>();
        tfName = nameBox.GetComponent<Transform>();

        // Gets info on the dialogue box components
        sp = GetComponent<SpriteRenderer>();
        tf = transform;
        height = sp.size.y;
        width = sp.size.x;
        sp.size = new Vector2(0, 0);
        spName.color = new Color(1, 1, 1, 0);
        path = "Assets/Resources/script.txt";
        script = File.ReadAllLines(path);
        at = -1;

    }

    // Update is called once per frame
    void Update()
    {
        // Opening dialogue box (width)
        if (growingW && sp.size.x < width)
        {
            sp.size = new Vector2((width + 0.5F) / 15 + sp.size.x, 0.5F);
            tf.position = new Vector3(tf.parent.position.x - (width - 0.5F) / 2 + (sp.size.x - 0.5F) / 2, tf.position.y, tf.position.z);
            if (sp.size.x >= width)
            {
                sp.size = new Vector2(width, 0.5F);
                tf.position = new Vector3(tf.parent.position.x, tf.position.y - 3.5F, tf.position.z);
                growingH = true;
            }
        }

        // Opening dialogue box (height)
        if (growingH && sp.size.y < height)
        {
            sp.size = new Vector2(width, (height - 0.5F) / 15 + sp.size.y);
            tf.position = new Vector3(tf.parent.position.x, tf.parent.position.y + (height - 0.5F) / 2 - (sp.size.y - 0.5F) / 2 - 3.5F, tf.position.z);
            if (sp.size.y >= height)
            {
                sp.size = new Vector2(width, height);
                tf.position = new Vector3(tf.parent.position.x, tf.parent.position.y - 3.5F, tf.position.z);
                reader = 1;
                checkCase();
            }
        }

        // Autoscroll Dialogue
        if (sp.size.y == height && reader <= toRead.Length)
        {
            int tmp = toRead.Length;
            for (int i = 0; i < txtSpeed && reader <= tmp; i++)
            {
                if (readcnt > 75)
                {
                    int cutoff = text.text.LastIndexOf("  ");
                    toRead = text.text.Substring(0, cutoff) + "\n" + text.text.Substring(cutoff + 2) + toRead.Substring(text.text.Length);
                    readcnt = 0;
                }

                text.text = toRead.Substring(0, (int)reader);
                readcnt++;
                reader++;
            }
        }

        // Next Dialogue Line
        if (Input.GetKeyDown(KeyCode.Return) && sp.size.y == height && reader > toRead.Length)
        {
            at++;
            toRead = script[at];

            checkCase();

            readcnt = 0;
            reader = 0;
        }
    }

    void readDialogue(int rowNum)
    {
        talking = true;
        tf.position = new Vector3(tf.parent.position.x - (width - 0.5F) / 2, tf.parent.position.y + (height - 0.5F) / 2 - 3.5F, tf.position.z);
        growingW = true;
        sp.size = new Vector2(0.5F, 0.5F);
        spName.color = new Color(1, 1, 1, 1);
        at = rowNum;
        toRead = script[at];
    }

    void checkCase()
    {
        // Switch Case to check what the next line wants
        bool finished = false;
        while (!finished)
        {
            switch (toRead.Substring(0, 5))
            {
                case "ENDS:":
                    // Resets all values to remove/reset the text box
                    growingH = growingW = false;
                    sp.size = new Vector2(0, 0);
                    text.text = "";
                    nametag.text = "";
                    spName.color = new Color(255, 255, 255, 0);
                    talking = false;
                    pic1.sprite = null;
                    pic2.sprite = null;
                    tfName.position = new Vector3(tfName.parent.position.x - 6.5F, tfName.position.y, tfName.position.z);
                    finished = true;
                    break;
                case "NAME:":
                    // Reposition name box
                    if (nametag.text != "")
                    {
                        if(tfName.position.x - tfName.parent.position.x < 0)
                            tfName.position = new Vector3(tfName.parent.position.x + 6.5F, tfName.position.y, tfName.position.z);
                        else
                            tfName.position = new Vector3(tfName.parent.position.x - 6.5F, tfName.position.y, tfName.position.z);
                    }
                    float col = 100F;
                    if (tfName.position.x - tfName.parent.position.x > 0)
                    {
                        pic1.color = new Color(col / 255F, col / 255F, col / 255F, 1);
                        pic2.color = new Color(255, 255, 255, 1);
                    }
                    else
                    {
                        pic1.color = new Color(255, 255, 255, 1);
                        pic2.color = new Color(col / 255F, col / 255F, col / 255F, 1);
                    }

                    nametag.text = toRead.Substring(5);

                    break;
                case "PIC1:":
                    pic1.sprite = Resources.Load<Sprite>("Sprites/" + toRead.Substring(5));
                    break;
                case "PIC2:":
                    pic2.sprite = Resources.Load<Sprite>("Sprites/" + toRead.Substring(5));
                    break;
                default:
                    finished = true;
                    break;
            }
            if (!finished)
            {
                at++;
                toRead = script[at];
            }
        }
    }
}
