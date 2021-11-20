using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private Vector2 velocity;

    public Transform follow;
    public float speed;
    public float xRange;
    public float yRange;
    public float xLimMin;
    public float xLimMax;
    public float yLimMin;
    public float yLimMax;

    private SpriteRenderer sprite;
    private bool shifting;
    private float shift;
    private const float shiftTime = 1.5F;
    private string nextScene;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        Movement.canMove = true;
        /*shifting = false;
        shift = shiftTime;
        sprite = blocker.GetComponent<SpriteRenderer>();
        sprite.color = new Color(0, 0, 0, 1);*/
        setBounds();
    }

    void FixedUpdate()
    {
        if (follow != null)
        {
            // Camera follow updates
            Vector2 toGo = transform.position;
            if (Mathf.Abs(toGo.x - follow.position.x) > xRange)
            {
                if (toGo.x > follow.position.x)
                    toGo.x = follow.position.x + xRange;
                else
                    toGo.x = follow.position.x - xRange;
            }

            if (Mathf.Abs(toGo.y - follow.position.y) > yRange)
            {
                if (toGo.y > follow.position.y)
                    toGo.y = follow.position.y + yRange;
                else
                    toGo.y = follow.position.y - yRange;
            }

            float x = Mathf.SmoothDamp(transform.position.x, toGo.x, ref velocity.x, speed);
            float y = Mathf.SmoothDamp(transform.position.y, toGo.y, ref velocity.y, speed);

            x = Mathf.Clamp(x, xLimMin, xLimMax);
            y = Mathf.Clamp(y, yLimMin, yLimMax);
            transform.position = new Vector3(x, y, transform.position.z);
        }
        

        if (shifting)
        {
            shift += Time.deltaTime;
            if (shift > shiftTime)
            {
                shift = shiftTime;
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            shift -= Time.deltaTime;
            if (shift < 0)
            {
                shift = 0;
                Movement.canMove = true;
            }
        }
        //sprite.color = new Color(0, 0, 0, shift / shiftTime);

        // Loading and quitting buttons
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Sets Player's bounds
    void setBounds()
    {
        if (follow != null)
        {
            follow.gameObject.SendMessage("setMinLimit", xLimMin - 8.5F);
            follow.gameObject.SendMessage("setMaxLimit", xLimMax + 8.5F);
            follow.gameObject.SendMessage("setMinHeight", yLimMin - 5.5F);
        }
    }

    // Changes Scenes
    void SceneShift(string scene)
    {
        shifting = true;
        nextScene = scene;
    }

    void ReloadScene()
    {
        SceneShift(SceneManager.GetActiveScene().name);
    }
}
