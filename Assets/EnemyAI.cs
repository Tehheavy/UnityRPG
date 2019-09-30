using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public GameObject PlayerAnimator;
    private Animator animator;
    public MapLoad tempMap;
    int[] currentPosition;
    private bool Moving;

    List<Vector2> PlayerFullPath = null;
    public float speed;
    Vector3 target;
    Vector3 prevtarget;
    Vector3 spawnlocation;
    float stepTime;
    private Vector3 myLocalscale;
    public bool stopped = false;
    private bool fighting = false;
    public int maxHealth;
    public int curHealth;
    public int level;
    private float ViewRange = 2.5f;
    public Canvas healthbarCanvas;
    public Image healhbar;
    public GameObject player;
    public EnemyPathing enemyPathing;
    private float TimeInterval = 0;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = (int)Mathf.Pow(level, 2) + 10;
        curHealth = maxHealth;
        spawnlocation = transform.position;
        prevtarget = transform.position;
        myLocalscale = transform.localScale;
        speed = 1.0f;
        Moving = false;
        stepTime = speed;
        healthbarCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimator.GetComponent<Animator>().SetBool("Moving", Moving);
        PlayerAnimator.GetComponent<Animator>().SetBool("Attacking", fighting);
        if (!stopped)
        {
            if (!fighting)
            {
                if (!Moving)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < ViewRange)
                    {
                        int[] end = new int[2] { (int)player.transform.position.z, (int)player.transform.position.x };
                        currentPosition = new int[2] { (int)transform.position.z, (int)transform.position.x };
                        List<Vector2> path = new Player.Astar(tempMap.mapArray, currentPosition, end, "DiagonalFree").result;
                        for (int i = 0; i < path.Count; i++)
                        {
                            path[i] = new Vector2(path[i].y, path[i].x);
                        }
                        if (path.Count > 1)
                        {
                            path.Remove(path[0]);

                            Debug.Log("path 0" + path[0]);
                            PlayerFullPath = path;
                            path = null;
                            Moving = true;
                            target.x = PlayerFullPath[0].x + 0.5f;
                            target.z = PlayerFullPath[0].y + 0.5f;
                            target.y = 0.5f;
                        }
                    }
                    if ((int)transform.position.x == (int)player.transform.position.x && (int)transform.position.z == (int)player.transform.position.z)
                    {
                        fight(player.GetComponent<Player>());
                    }
                }
                if (Moving)
                {
                    Debug.Log("Ai  TEST12");
                    transform.position = Vector3.MoveTowards(transform.position, target, stepTime * Time.deltaTime);
                    if (transform.position.x < target.x)
                    {
                        myLocalscale.x = -1;
                        transform.localScale = myLocalscale;
                    }
                    else if (transform.position.x > target.x)
                    {
                        myLocalscale.x = 1;
                        transform.localScale = myLocalscale;
                    }
                }
                if (PlayerFullPath != null)
                {
                    if (PlayerFullPath.Count > 0)
                    {
                        Debug.Log("Player Path Count=" + PlayerFullPath.Count);
                        if (transform.position == target)
                        {
                            prevtarget = target;
                            PlayerFullPath.Remove(PlayerFullPath[0]);
                            if (PlayerFullPath.Count > 0)
                            {
                                target.x = PlayerFullPath[0].x + 0.5f;
                                target.z = PlayerFullPath[0].y + 0.5f;
                                target.y = 0.5f;
                            }
                        }
                    }
                    else
                    {
                        Moving = false;
                    }

                }
            }
            else
            {// if fighting
                Debug.Log("moving" + Moving);
            }
        }
        else
        {
            if (spawnlocation != null && transform.position != spawnlocation)
            {
                transform.position = Vector3.MoveTowards(transform.position, spawnlocation, stepTime * Time.deltaTime);
            }
        }

    }

    void LateUpdate()
    {
        // ones per in seconds
        TimeInterval += Time.deltaTime;
        if (fighting)
        {
            if (TimeInterval >= 1)
            {
                Player tempPlayer = player.GetComponent<Player>();
                takeDamage(1);
                giveDamage(tempPlayer, (int)((float)maxHealth * 0.1f));
                TimeInterval = 0;
                // Performance friendly code here
            }
        }
    }
    public virtual void fight(Player player)
    {
        foreach (GameObject item in enemyPathing.enemies)
        {
            if (this.gameObject != item)
            {
                // item.SetActive(false);
                EnemyAI test = item.GetComponent<EnemyAI>();
                test.target = prevtarget;
                test.stopped = true;
                Debug.Log("OTHER ITEM MET PLAYER BEFORE ME");
            }
        }
        this.fighting = true;
        player.Fighting = true;
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        enemyPosition.x = enemyPosition.x + 0.4f;
        playerPosition.x = playerPosition.x - 0.4f;
        myLocalscale.x = 1;
        transform.localScale = myLocalscale;
        player.transform.position = playerPosition;
        transform.position = enemyPosition;
        // Debug.LogError("Fighting Player"+player.speed);
    }

    public virtual void takeDamage(int value)
    {
        curHealth = curHealth - value;
        if (curHealth > 0)
        {

            healthbarCanvas.enabled = true;
            healhbar.fillAmount = (float)curHealth / (float)maxHealth;
        }
        else
        {//  if (curHealth < 0)
            curHealth = 0;
            Player tempPlayer = player.GetComponent<Player>();

            endfight(tempPlayer);
        }


    }
    public virtual void giveDamage(Player player, int damage)
    {
        player.updateHp(damage);
    }
    public virtual void endfight(Player player)
    {
        foreach (GameObject item in enemyPathing.enemies)
        {
            if (this.gameObject != item)
            {
                // item.SetActive(false);
                EnemyAI test = item.GetComponent<EnemyAI>();
                test.stopped = false;
                Debug.Log("OTHER ITEM MET PLAYER BEFORE ME");
            }
        }
        this.fighting = false;
        player.Fighting = false;
        Vector3 playerPosition = player.transform.position;
        playerPosition.x = playerPosition.x + 0.4f;
        player.transform.position = playerPosition;
        player.updateXp((int)((float)this.maxHealth / 2f));

        enemyPathing.enemies.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
