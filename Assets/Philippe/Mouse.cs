using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MouseState
{
    Controllable, Uncontrollable
}

public class Mouse : MonoBehaviour
{
    //=========================================================================================================
    // V A R I A B L E S
    [Header("Player Settings")]
    public int max_health = 8;
    public int curr_health = 8;

    [Header("Visual Settings")]
    public Sprite[] sprites;
    public GameObject cursor_visual;
    private SpriteRenderer sprite_renderer;

    [Header("Control Settings")]
    public float mouse_sensitivity_x = 0.34f;
    public float mouse_sensitivity_y = 0.34f;
    public float bound_x = 6f;
    public float bound_y = 4.5f;
    public GameObject bound_x_box;
    [SerializeField] MouseState mouseState;

    private Vector3 saved_point = Vector3.zero;
    private Vector3 velocity;
    private Vector3 velocity_vec;
    private float time = 0f;
    public float timeFactor = 4f;
    public float distFactor = 0.5f;
    private Vector3 start_point;
    private Vector3 end_point;
    public AnimationCurve animationCurve;
    public CameraController cameraController;
    public Vector2 ss_values;
    private GameManager gameManager;
    public HealthBar healthBar;
    public EnergyBar energyBar;
    public TrailRenderer trailRenderer;
    public ClippingScript clippingScript;
    public Material pixelMaterial;

    private float x_pos = 0, y_pos = 0;
    private Collider2D collider_2D;

    //Spherecast vector
    float radius = 0.15f;

    //Powerup Variables
    int dirValue = 1;
    bool SMASH = false;
    //=========================================================================================================
    // F U N C T I O N S

    void setState(MouseState in_mouse_state)
    {
        mouseState = in_mouse_state;
        sprite_renderer.sprite = sprites[(int)in_mouse_state];
    }

    void Start()
    {
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        collider_2D = cursor_visual.GetComponent<BoxCollider2D>();
        sprite_renderer = cursor_visual.GetComponent<SpriteRenderer>();
        mouseState = MouseState.Controllable;
        //x_pos = 0;
        //y_pos = 0;

        bound_x = Camera.main.orthographicSize * Screen.width / Screen.height;
        bound_y = Camera.main.orthographicSize;
    }
    
    void Update()
    {
        if(gameManager.timeSlow == true)
        {
            trailRenderer.time = 40f;
        }
        else
        {
            trailRenderer.time = 0.2f;
        }

        bound_x = bound_x_box.transform.position.x;
        //USE MOUSESTATE TO DETERMINE WHAT TO DO
        if (mouseState == MouseState.Controllable)
        {
            //Test for colliders
            Collider2D[] colliders = Physics2D.OverlapCircleAll(cursor_visual.transform.position + new Vector3(-0.06f, 0f, 0), radius);
            if (colliders.Length > 0)
            {
                if (colliders[0].gameObject.layer == LayerMask.NameToLayer("Enemy") && gameManager.timeSlow == false)
                {
                    colliders[0].gameObject.GetComponent<enemycontroller>().KillSquarePassive();
                    healthBar.hp_change(-1);
                }
                else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("Enemy") && gameManager.timeSlow == true)
                {
                    //colliders[0].gameObject.GetComponent<enemycontroller>().KillSquare(transform.position);
                }
                else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("Armor"))
                {
                    cameraController.screen_shake(ss_values.x, ss_values.y);
                }
                else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    healthBar.hp_change(-1);
                    Destroy(colliders[0].gameObject);
                }
                /*
                else if (colliders[0].gameObject.layer ==LayerMask.NameToLayer("Powerup"))
                {
                    GameObject powerup = colliders[0].gameObject;
                    powerup.GetComponent<powerUpInterface>().powerUp();
                }
                */
            }

            //increase yaw (xaxis) by the mouse's X pos and multiply that by the mouse sentitivity
            x_pos += Input.GetAxis("Mouse X") * mouse_sensitivity_x * Time.deltaTime; //* dirValue;
            y_pos += Input.GetAxis("Mouse Y") * mouse_sensitivity_y * Time.deltaTime;

            //Clamp mouse position to bounds of the screen
            x_pos = Mathf.Clamp(x_pos, -bound_x, bound_x);
            y_pos = Mathf.Clamp(y_pos, -bound_y, bound_y);

            //Move the cursor to the new location
            Vector3 start_vec = cursor_visual.transform.position;// + new Vector3(-0.1f, 0.01f, 0);
            Vector3 end_vec = new Vector3(x_pos, y_pos, 0);// + new Vector3(-0.1f, 0.01f, 0);
            RaycastHit2D hit = Physics2D.Raycast(start_vec, end_vec, 0.3f);
            if (hit.collider != null)
            {
                LayerMask layer = hit.collider.gameObject.layer;
                if (layer == LayerMask.NameToLayer("Enemy"))
                {
                    if(gameManager.timeSlow == true)
                    {
                        cameraController.screen_shake(ss_values.x, ss_values.y);
                        GameObject enemy = hit.collider.gameObject;
                        enemycontroller enemyController = enemy.GetComponent<enemycontroller>();
                        enemyController.cut.updateCorners();
                        Vector3 v3 = (end_vec - start_vec);
                        List<Vector2> clips = clippingScript.clips(cursor_visual.transform.position - v3.normalized, cursor_visual.transform.position + (v3).normalized * 10f, hit.collider.gameObject.GetComponent<PolygonCollider2D>());

                        if(clips.Count > 1)
                        {
                            enemyController.KillSquare(clips[0], clips[1]);
                        }
                        cursor_visual.transform.position = new Vector3(x_pos, y_pos, 0);
                    }
                    
                }
                else if (layer == LayerMask.NameToLayer("Armor"))
                {
                    cursor_visual.transform.position = hit.point;
                    start_point = hit.point;
                    x_pos = hit.point.x;
                    y_pos = hit.point.y;
                    Vector3 hit2_point = hit.point;
                    end_point = (hit2_point - hit.collider.transform.position).normalized * distFactor + hit2_point;
                    time = 0;
                    setState(MouseState.Uncontrollable);
                }
                else
                {
                    cursor_visual.transform.position = new Vector3(x_pos, y_pos, 0);
                }
                /*
                else if (layer == LayerMask.NameToLayer("Chunk"))
                {
                    cursor_visual.transform.position = new Vector3(x_pos, y_pos, 0);
                    GameObject enemy = hit.collider.gameObject;
                    Cut cut = enemy.GetComponent<Cut>();
                    cut.updateCorners();
                    Vector3 v3 = (end_vec - start_vec);
                    List<Vector2> clips = clippingScript.clips(cursor_visual.transform.position - v3.normalized, cursor_visual.transform.position + (v3).normalized * 10f, hit.collider.gameObject.GetComponent<PolygonCollider2D>());
                    cut.inMaterial = pixelMaterial;
                    if (clips.Count > 1 && cut.canBeCut == true)
                    {
                        cut.mouseEnter = clips[0];
                        cut.mouseExit = clips[1];
                        cut.cutGameObject();
                    }
                }
                */
            }
            else
            {
                cursor_visual.transform.position = new Vector3(x_pos, y_pos, 0);
            }
            
            //VELOCITY
            velocity = (cursor_visual.transform.position - saved_point) / Time.deltaTime;
            saved_point = cursor_visual.transform.position;

            
        }
        else if(mouseState == MouseState.Uncontrollable)
        {
            time += Time.deltaTime * timeFactor;
            if(time > 1)
            {
                setState(MouseState.Controllable);
            }
            else
            {
                Vector3 temp = Vector3.Lerp(start_point, end_point, animationCurve.Evaluate(time));
                x_pos = temp.x;
                y_pos = temp.y;
                x_pos = Mathf.Clamp(x_pos, -bound_x, bound_x);
                y_pos = Mathf.Clamp(y_pos, -bound_y, bound_y);
                cursor_visual.transform.position = new Vector3(x_pos, y_pos, 0);
            }
        }
    }


    //Mirrormouse powerup stuff
    public void reverseMouse()
    {
        if(dirValue == 1)
        {
            print("negative");
            dirValue = -1;
        } else
        {
            dirValue = 1;
        }
    }

    IEnumerator killSelf(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(transform.parent.gameObject);
    }

    public void triggerKill(int time)
    {
        StartCoroutine(killSelf(time));
    }

    public void teleportMouse(float x, float y)
    {
        x_pos = x;
        y_pos = y;
    }
    
    //SMASH powerup

    public void toggleSmash()
    {
        if(!SMASH)
        {
            radius = 0.3f;
           // transform.Find("Cursor_Visual").localScale = new Vector3(10, 10, 10);
            
            SMASH = true;
        } else
        {
            print("unsmash");
            SMASH = false;
            radius = 0.15f;
            transform.localScale /= 2;
        }

    }

}
