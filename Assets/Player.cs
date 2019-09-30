using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject PlayerAnimator;
    private Animator animator;
    public LayerMask ClickArea;
    public GameObject myMap;
    public MapLoad tempMap;
    public Material pathMaterial;

    int[] currentPosition;
    private bool Moving;

    [SerializeField]
    private int experience=0;
    private int[] expLevelReq={10,20,40,100,200,250,350,500,1000,1500,2000,3000};
    private int currentLevel=1;

    public Image hpSlider;
    private int maxPlayerHp=100;
    private int curPlayerHp=100;
    private int maxPlayerMp=100;
    private int curPlayerMp=100;
    public Image mpSlider;
    public Image xpSlider;
    List<Vector2> PlayerFullPath;
    public float speed;
    private int step = 0;
    private bool fighting = false;
    public bool Fighting
    {

        get { return fighting; }

        set { fighting = value; }

    }
    private int stepAnimation = 0;
    Vector3 target;
    float stepTime;
    private Vector3 myLocalscale;



    // Start is called before the first frame update
    void Start()
    {
        PlayerFullPath = new List<Vector2>();
        myLocalscale = transform.localScale;
        speed = 1.5f;
        Moving = false;
        stepTime = speed;
        


    }

    // Update is called once per frame
    void Update()
    {
             PlayerAnimator.GetComponent<Animator>().SetBool("Moving", Moving);
      //  Debug.Log("check moving" + Moving);

        if (!fighting)
        {
            //   Debug.Log("x:" + transform.position.x + " z:" + transform.position.z + "step :" + step + " Moving: " + Moving);
            currentPosition = new int[2] { (int)transform.position.z, (int)transform.position.x };
            if (tempMap.mapArray[(int)transform.position.x][(int)transform.position.z] == 0)
                if (Input.GetMouseButtonDown(0))
                {

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    // Casts the ray and get the first game object hit
                    Physics.Raycast(ray, out hit);
                    Debug.Log("This hit at " + hit.point + " THIS POINT IS value = " + tempMap.mapArray[(int)hit.point.x][(int)hit.point.z]);
                    if (hit.point.x != 0 && hit.point.z != 0&&tempMap.mapArray[(int)hit.point.x][(int)hit.point.z]!=1)
                    {
                        int[] end = new int[2] { (int)hit.point.z, (int)hit.point.x };
                        if (!((int)hit.point.x == (int)transform.position.x && (int)hit.point.z == (int)transform.position.z))
                        {
                            //     Debug.LogError("my x:"+(int)transform.position.x+" target x: "+(int)hit.point.x);
                            Debug.Log("Entered THIS");
                            List<Vector2> path = new Astar(tempMap.mapArray, currentPosition, end, "DiagonalFree").result;
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
                        else
                        {
                            Debug.Log("Error");
                        }
                    }


                }

            if (Moving)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, stepTime * Time.deltaTime);
                if (transform.position.x < target.x)
                {
                    myLocalscale.x = 1;
                    transform.localScale = myLocalscale;
                }
                else if (transform.position.x > target.x)
                {
                    myLocalscale.x = -1;
                    transform.localScale = myLocalscale;
                }
            }
            if (PlayerFullPath.Count > 0)
            {
                Debug.Log("Player Path Count=" + PlayerFullPath.Count);
                if (transform.position == target)
                {
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

    public bool cmpVect3Vect2(Vector3 var1, Vector2 var2)
    {
        if (var1.x == var2.x && var1.z == var2.y)
            return true;
        return false;
    }
    public void updateXp(int value){
        this.experience+=value;
        if(this.experience>=expLevelReq[currentLevel-1]){
            //levelup
            experience=0;
            currentLevel+=1;
            xpSlider.fillAmount=0;
        }
        else
           xpSlider.fillAmount = (float)this.experience/(float)expLevelReq[currentLevel-1];
    }
    public void updateHp(int damage){
        this.curPlayerHp-=damage;
        if(this.curPlayerHp<=0){
            //gameover();
        }
        else
            hpSlider.fillAmount=(float)this.curPlayerHp/(float)maxPlayerHp;


    }
    public void updateMp(int value){
        
    }
    public class Astar
    {
        /*
    Unity C# Port of Andrea Giammarchi's JavaScript A* algorithm (http://devpro.it/javascript_id_137.html)
    Usage:

    int[][] map = new int[][] 
    {
        new int[] {0, 0, 0, 0, 0, 0, 0, 0},
        new int[] {0, 0, 0, 0, 0, 0, 0, 0},	
        new int[] {0, 0, 0, 1 @, 0, 0, 0, 0},//[2][3] = 1 in this algorithm, for me :
        new int[] {0, 0, 0 @, 1, 0, 0, 0, 0}, [2][3]=0
        new int[] {0, 0, 0, 1, 0, 0, 0, 0},
        new int[] {1, 0, 1, 0, 0, 0, 0, 0},
        new int[] {1, 0, 1, 0, 0, 0, 0, 0},
        new int[] {1, 1, 1, 1, 1, 1, 0, 0},
        new int[] {1, 0, 1, 0, 0, 0, 0, 0},
        new int[] {1, 0, 1, 2, 0, 0, 0, 0}
    };
    int[] start	= new int[2] {0, 0};
    int[] end	= new int[2] {5, 5};
    List<Vector2> path = new Astar(map, start, end, "DiagonalFree").result;
    */
        public List<Vector2> result = new List<Vector2>();
        private string find;

        private class _Object
        {
            public int x
            {
                get;
                set;
            }
            public int y
            {
                get;
                set;
            }
            public double f
            {
                get;
                set;
            }
            public double g
            {
                get;
                set;
            }
            public int v
            {
                get;
                set;
            }
            public _Object p
            {
                get;
                set;
            }
            public _Object(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private _Object[] diagonalSuccessors(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
        {
            if (xN)
            {
                if (xE && grid[N][E] == 0)
                {
                    result[i++] = new _Object(E, N);
                }
                if (xW && grid[N][W] == 0)
                {
                    result[i++] = new _Object(W, N);
                }
            }
            if (xS)
            {
                if (xE && grid[S][E] == 0)
                {
                    result[i++] = new _Object(E, S);
                }
                if (xW && grid[S][W] == 0)
                {
                    result[i++] = new _Object(W, S);
                }
            }
            return result;
        }

        private _Object[] diagonalSuccessorsFree(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
        {
            xN = N > -1;
            xS = S < rows;
            xE = E < cols;
            xW = W > -1;

            if (xE)
            {
                if (xN && grid[N][E] == 0)
                {
                    result[i++] = new _Object(E, N);
                }
                if (xS && grid[S][E] == 0)
                {
                    result[i++] = new _Object(E, S);
                }
            }
            if (xW)
            {
                if (xN && grid[N][W] == 0)
                {
                    result[i++] = new _Object(W, N);
                }
                if (xS && grid[S][W] == 0)
                {
                    result[i++] = new _Object(W, S);
                }
            }
            return result;
        }

        private _Object[] nothingToDo(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
        {
            return result;
        }

        private _Object[] successors(int x, int y, int[][] grid, int rows, int cols)
        {
            int N = y - 1;
            int S = y + 1;
            int E = x + 1;
            int W = x - 1;

            bool xN = N > -1 && grid[N][x] == 0;
            bool xS = S < rows && grid[S][x] == 0;
            bool xE = E < cols && grid[y][E] == 0;
            bool xW = W > -1 && grid[y][W] == 0;

            int i = 0;

            _Object[] result = new _Object[8];

            if (xN)
            {
                result[i++] = new _Object(x, N);
            }
            if (xE)
            {
                result[i++] = new _Object(E, y);
            }
            if (xS)
            {
                result[i++] = new _Object(x, S);
            }
            if (xW)
            {
                result[i++] = new _Object(W, y);
            }

            _Object[] obj =
                (this.find == "Diagonal" || this.find == "Euclidean") ? diagonalSuccessors(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
                (this.find == "DiagonalFree" || this.find == "EuclideanFree") ? diagonalSuccessorsFree(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
                                                                                         nothingToDo(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i);

            return obj;
        }

        private double diagonal(_Object start, _Object end)
        {
            return Math.Max(Math.Abs(start.x - end.x), Math.Abs(start.y - end.y));
        }

        private double euclidean(_Object start, _Object end)
        {
            var x = start.x - end.x;
            var y = start.y - end.y;

            return Math.Sqrt(x * x + y * y);
        }

        private double manhattan(_Object start, _Object end)
        {
            return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
        }

        public Astar(int[][] grid, int[] s, int[] e, string f)
        {
            this.find = (f == null) ? "Diagonal" : f;

            int cols = grid[0].Length;
            int rows = grid.Length;
            int limit = cols * rows;
            int length = 1;

            List<_Object> open = new List<_Object>();
            open.Add(new _Object(s[0], s[1]));
            open[0].f = 0;
            open[0].g = 0;
            open[0].v = s[0] + s[1] * cols;

            _Object current;

            List<int> list = new List<int>();

            double distanceS;
            double distanceE;

            int i;
            int j;

            double max;
            int min;

            _Object[] next;
            _Object adj;

            _Object end = new _Object(e[0], e[1]);
            end.v = e[0] + e[1] * cols;

            bool inList;

            do
            {
                max = limit;
                min = 0;

                for (i = 0; i < length; i++)
                {
                    if (open[i].f < max)
                    {
                        max = open[i].f;
                        min = i;
                    }
                }

                current = open[min];
                open.RemoveAt(min);

                if (current.v != end.v)
                {
                    --length;
                    next = successors(current.x, current.y, grid, rows, cols);

                    for (i = 0, j = next.Length; i < j; ++i)
                    {
                        if (next[i] == null)
                        {
                            continue;
                        }

                        (adj = next[i]).p = current;
                        adj.f = adj.g = 0;
                        adj.v = adj.x + adj.y * cols;
                        inList = false;

                        foreach (int key in list)
                        {
                            if (adj.v == key)
                            {
                                inList = true;
                            }
                        }

                        if (!inList)
                        {
                            if (this.find == "DiagonalFree" || this.find == "Diagonal")
                            {
                                distanceS = diagonal(adj, current);
                                distanceE = diagonal(adj, end);
                            }
                            else if (this.find == "Euclidean" || this.find == "EuclideanFree")
                            {
                                distanceS = euclidean(adj, current);
                                distanceE = euclidean(adj, end);
                            }
                            else
                            {
                                distanceS = manhattan(adj, current);
                                distanceE = manhattan(adj, end);
                            }

                            adj.f = (adj.g = current.g + distanceS) + distanceE;
                            open.Add(adj);
                            list.Add(adj.v);
                            length++;
                        }
                    }
                }
                else
                {
                    i = length = 0;
                    do
                    {
                        this.result.Add(new Vector2(current.x, current.y));
                    }
                    while ((current = current.p) != null);
                    result.Reverse();
                }
            }
            while (length != 0);
        }
    }
}
