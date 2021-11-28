using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    public static bool canTalk;

    public TextMesh text;
    public TextMesh nametag;
    public SpriteRenderer pic1;
    public SpriteRenderer pic2;
    public SpriteRenderer screenBlock;
    public AudioSource song;
    public GameObject nameBox;
    public GameObject canvas;
    public GameObject contPrompt;
    public int txtSpeed;

    private SpriteRenderer sp;
    private SpriteRenderer spName;
    private Transform tfName;
    private Transform tf;
    private AudioSource auso;
    private AudioSource tick;
    private float height;
    private float width;
    private int size;
    private int at;
    private bool growingH;
    private bool growingW;
    private string[] script;
    private string toRead;
    private int reader;
    private int readcnt;
    private bool talking;
    private bool sounding;
    private int waitTime;

    // Start is called before the first frame update
    void Start()
    {
        talking = false;

        // Name Box Components
        spName = nameBox.GetComponent<SpriteRenderer>();
        tfName = nameBox.GetComponent<Transform>();
        nameBox.SetActive(false);

        // Sound effects
        auso = Camera.main.GetComponent<AudioSource>();

        // Gets info on the dialogue box components
        sp = GetComponent<SpriteRenderer>();
        tick = GetComponent<AudioSource>();
        tf = transform;
        height = sp.size.y;
        width = sp.size.x;
        sp.size = new Vector2(0, 0);
        spName.color = new Color(1, 1, 1, 0);
        script = File.ReadAllLines("Assets/Resources/script.txt");
        at = -1;

    }

    // Update is called once per frame
    void Update()
    {

        canTalk = !talking;
        // Opening dialogue box (width)
        if (Movement.grounded && growingW && sp.size.x < width)
        {
            Time.timeScale = 0;
            Movement.canMove = false;
            sp.size = new Vector2((width + 0.5F) / 15 + sp.size.x, 0.5F);
            tf.position = new Vector3(tf.parent.position.x - (width - 0.5F) / 2 + (sp.size.x - 0.5F) / 2, tf.position.y, tf.position.z);
            if (sp.size.x >= width)
            {
                sp.size = new Vector2(width, 0.5F);
                tf.position = new Vector3(tf.parent.position.x, tf.position.y - 5F, tf.position.z);
                growingH = true;
            }
        }

        // Opening dialogue box (height)
        if (growingH && sp.size.y < height)
        {
            sp.size = new Vector2(width, (height - 0.5F) / 15 + sp.size.y);
            tf.position = new Vector3(tf.parent.position.x, tf.parent.position.y + (height - 0.5F) / 2 - (sp.size.y - 0.5F) / 2 - 5F, tf.position.z);
            if (sp.size.y >= height)
            {
                sp.size = new Vector2(width, height);
                tf.position = new Vector3(tf.parent.position.x, tf.parent.position.y - 5F, tf.position.z);
                reader = 1;
                checkCase();
            }
        }

        // Audio Source check
        if(sounding && !auso.isPlaying)
        {
            sounding = false;
            checkCase();
        }

        // Wait Time Check
        if(waitTime > 0)
        {
            waitTime--;
            if(waitTime == 0)
                checkCase();
        }


        // Autoscroll Dialogue
        if (sp.size.y == height && reader <= toRead.Length && !sounding && waitTime == 0)
        {
            int tmp = toRead.Length;
            for (int i = 0; i < txtSpeed && reader <= tmp; i++)
            {
                if (readcnt > 90)
                {
                    int cutoff = text.text.LastIndexOf(" ");
                    if (cutoff >= 0)
                        toRead = text.text.Substring(0, cutoff) + "\n" + text.text.Substring(cutoff + 1) + toRead.Substring(text.text.Length);
                    else
                        toRead = text.text + "\n" + toRead.Substring(text.text.Length);
                    readcnt = 0;
                }

                text.text = toRead.Substring(0, (int)reader);
                readcnt++;
                reader++;
            }
            if(!tick.isPlaying)
                tick.Play();
        }

        if (sp.size.y == height && reader > toRead.Length)
            contPrompt.SetActive(true);

        // Next Dialogue Line
        if (Input.GetKeyDown(KeyCode.Return) && sp.size.y == height && reader > toRead.Length)
        {
            contPrompt.SetActive(false);

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
        tf.position = new Vector3(tf.parent.position.x - (width - 0.5F) / 2, tf.parent.position.y + (height - 0.5F) / 2 - 5F, tf.position.z);
        growingW = true;
        sp.size = new Vector2(0.5F, 0.5F);
        spName.color = new Color(1, 1, 1, 1);
        canvas.SetActive(false);
        /*for(int i = 0; i < script.Length; i++)
        {
            print(i + "       " + script[i]);
        }*/
        at = rowNum;
        toRead = script[at];
    }

    void checkCase()
    {
        // Switch Case to check what the next line wants
        bool finished = false;
        while (toRead.Length >= 5 && !finished)
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
                    canvas.SetActive(true);
                    nameBox.SetActive(false);
                    talking = false;
                    pic1.sprite = null;
                    pic2.sprite = null;
                    tfName.position = new Vector3(tfName.parent.position.x - 9.5F, tfName.position.y, tfName.position.z);
                    finished = true;
                    Time.timeScale = 1;
                    Movement.canMove = true;
                    break;
                case "NAME:":
                    if(toRead.Substring(5) != "")
                        nameBox.SetActive(true);
                    else
                        nameBox.SetActive(false);

                    // Reposition name box
                    if (nametag.text != "")
                    {
                        if (tfName.position.x - tfName.parent.position.x < 0)
                            tfName.position = new Vector3(tfName.parent.position.x + 9.5F, tfName.position.y, tfName.position.z);
                        else
                            tfName.position = new Vector3(tfName.parent.position.x - 9.5F, tfName.position.y, tfName.position.z);
                    }
                    else
                        tfName.position = new Vector3(tfName.parent.position.x - 9.5F, tfName.position.y, tfName.position.z);
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
                case "NOIS:":
                    auso.clip = Resources.Load<AudioClip>("Sounds/" + toRead.Substring(5));
                    auso.Play();
                    sounding = true;
                    at++;
                    toRead = script[at];
                    finished = true;
                    break;
                case "FLTR:":
                    string[] colors = toRead.Substring(5).Split(',');
                    screenBlock.color = new Color(int.Parse(colors[0]) / 255F, int.Parse(colors[1]) / 255F, int.Parse(colors[2]) / 255F, float.Parse(colors[3]));
                    break;
                case "SONG:":
                    string[] command = toRead.Substring(5).Split(',');
                    song.clip = Resources.Load<AudioClip>("Sounds/" + command[1]);
                    if (command[0] == "Play")
                        song.Play();
                    else
                        song.Stop();
                    break;
                case "WAIT:":
                    waitTime = (int) float.Parse(toRead.Substring(5)) * 60;
                    at++;
                    toRead = script[at];
                    finished = true;
                    break;
                case "EMPT:":
                    text.text = "";
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
