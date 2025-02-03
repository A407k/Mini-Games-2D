using UnityEngine;
using TMPro;
using UnityEngine.Pool;



public class BB_Main : MonoBehaviour
{

    [Tooltip("Prefab of Bouncing Ball")]
    [SerializeField] private BouncingBall BB_Prefab;

    // The BB object pool
    private IObjectPool<BouncingBall> _bouncingBallPool;

    // Just there to be false
    [SerializeField] private bool collectionCheck = false;

    [SerializeField] private int defaultCapacity = 10;
    [SerializeField]private int maxCapacity = 25;


    [SerializeField]
    private TextMeshProUGUI Text_Score; 

    [SerializeField]
    private TextMeshProUGUI Text_HighScore;

    [SerializeField]
    private int[] array_Score; // Increase BB life by 1 acc. to this array


    private int Score =0; // to store the score

    private int highScore; // to store the HighScore

    private const string HighScoreKey = "CW_HighScore";

    private int bb_lifeCap = 1; 




    private void Awake()
    {
        // Initialize the Pool
        _bouncingBallPool = new ObjectPool<BouncingBall>(
            create_BB,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck,
            defaultCapacity,
            maxCapacity);
    }

    private BouncingBall create_BB()
    {
        BouncingBall BB_Instance = Instantiate(BB_Prefab);
        BB_Instance.ObjectPool = _bouncingBallPool;
        return BB_Instance;
    }

    private void OnGetFromPool(BouncingBall pooledBall)
    {
        pooledBall.gameObject.SetActive(true);
       
    }

    private void OnReleaseToPool(BouncingBall pooledBall)
    {
        pooledBall?.gameObject.SetActive(false);

        // After releasing call another BB
        callBB();

        // update score in script
        Score = Score + pooledBall.starting_life;

        // update the displayed score
        updateScore();

        // call another bb in its own place if
        if (pooledBall.starting_life % 3 == 0)
        {
            callBB_formBB(pooledBall.starting_life / 2, pooledBall.transform);
        }

    }

    private void OnDestroyPooledObject(BouncingBall pooledBall)
    {
        Destroy(pooledBall.gameObject);
    }



    private void Start()
    {
        // Load HighScore when the scene starts
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // initial score display when scene starts
        updateScore();

        // start game by calling the first bb
        Invoke("callBB", 3f);      

    }

    private void callBB()
    {
        // Get a ball from the pool
        BouncingBall Ball = _bouncingBallPool.Get();

        // set bb's stats
        Ball.setBB_life(bb_lifeCap);

        // Set the ball's position to the current object's position
        Ball.transform.position = gameObject.transform.position;
        
    }

    private void callBB_formBB(int x, Transform bb_t)
    {
        // Get a ball from the pool
        BouncingBall Ball = _bouncingBallPool.Get();

        // set bb's stats
        Ball.setBB_life(x);

        // Set the ball's position to the current object's position
        Ball.transform.position = bb_t.position;

    }

    private void updateScore()
    {
        // display score
        if(Text_Score != null)
        {
            Text_Score.text = "Score: " + Score.ToString();
        }

        // update highscore in script
        updateHighScore();

        // display highscore
        if (Text_HighScore != null)
        {
            Text_HighScore.text = "HighScore: " + highScore.ToString();
        }

        // explained
        updatebb_lifeCap();

    }

    private void updatebb_lifeCap()
    {
        // increase life cap if score reached a certain number
        if(System.Array.Exists(array_Score, element => element == Score))
        {
            bb_lifeCap++;
        }
    }

    private void updateHighScore()
    {
        if (Score > highScore)
        {
            highScore = Score;

            // save highscore in persistant storage
            SaveHighScore();
        }
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }


}
