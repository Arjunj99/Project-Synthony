using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemycontroller : MonoBehaviour
{
    private float personalTime = 0;
    public float delayDash = 1f;
    public float dashSpeed = 6f;
    private float dashTime;
    bool notDash = true;
    private GameManager gameManager;
    private EnemySpawn enemySpawn;
    public Cut cut;
    private GameObject player;
    [SerializeField] AnimationCurve dashCurve;
    Vector3 start_pos, end_pos;
    float elapsed;
    private void Start()
    {
        delayDash = Random.Range(0.5f, 1.5f);
        dashTime = delayDash;
        dashSpeed = Random.Range(4f, 8f);
        transform.localScale = Vector3.one * Random.Range(0.4f, 0.8f);
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
        enemySpawn = GameObject.Find("Main").GetComponent<EnemySpawn>();
        cut = GetComponent<Cut>();
        if (gameManager.gameOver == false)
        {
            player = GameObject.Find("Cursor").transform.GetChild(1).gameObject;
        }
    }
    
    void Update()
    {
        personalTime += gameManager.globalTimeDelta;

        if(personalTime > delayDash && notDash && gameManager.gameOver == false)
        {
            transform.up = player.transform.position - transform.position;
            start_pos = transform.position;
            end_pos = (player.transform.position - transform.position) * 1.2f + transform.position + Vector3.right * Random.Range(-0.5f, 0.5f) + Vector3.up * Random.Range(-0.5f, 0.5f);
            elapsed = 0.0f;
            dashTime = (end_pos-start_pos).magnitude / dashSpeed;
            notDash = !notDash;
        }
        else if(personalTime > delayDash && !notDash && gameManager.gameOver == false)
        {
            elapsed += gameManager.globalTimeDelta;
            //transform.position += transform.up * dashSpeed * gameManager.globalTimeDelta; //* dashCurve.Evaluate(elapsed/ (end_pos - start_pos).magnitude/dashSpeed);
            transform.position = Vector3.Lerp(start_pos, end_pos, dashCurve.Evaluate(elapsed / dashTime));
            if(elapsed > dashTime)
            {
                personalTime = 0;
                notDash = !notDash;
            }
        }
    }

    public void KillSquare(Vector3 cursor_position, Vector3 out_position)
    {
        //WRITE WHAT YOU WANT TO HAPPEN HERE
        //enemySpawn.SpawnRand();
        gameManager.audioManager.OnEnemyKill();
        gameManager.highScore++;
        cut.mouseEnter = cursor_position;
        cut.mouseExit = out_position;
        cut.cutGameObject();
        gameManager.energyBar.energy_change(0.2f);
        //DESTROY AT THE END
    }

    public void KillSquarePassive()
    {
        Destroy(gameObject);
    }
}
