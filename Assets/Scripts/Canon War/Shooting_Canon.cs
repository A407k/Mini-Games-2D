using System;
using UnityEngine;
using UnityEngine.Pool;

public class Shooting_Canon : MonoBehaviour
{
    [Tooltip("Prefab of CanonBall")]
    [SerializeField] private CanonBall CB_Prefab;

    [Tooltip("CanonBall Speed")]
    [SerializeField] private float CB_Velocity = 100f;

    [Tooltip("CanonBall Spawn Position")]
    [SerializeField] private Transform Canon_Transform;

    [Tooltip("Canon Fire Speed")]
    [SerializeField] private float Canon_CoolDownWindow = 1f;

    private IObjectPool<CanonBall> canonBallPool;

    [SerializeField] private bool collectionCheck = false;

    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxCapacity = 100;

    private float nextTimeToShoot;

    // reference to script potentially its temporary
    private Move_Canon mc;

  


    private void Awake()
    {
        // initialize the pool
        canonBallPool = new ObjectPool<CanonBall>(
            Create_CB,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck,
            defaultCapacity,
            maxCapacity);

    }

    private CanonBall Create_CB()
    {
        CanonBall CB_Instance = Instantiate(CB_Prefab);
        CB_Instance.ObjectPool = canonBallPool;
        return CB_Instance;
    }

    private void OnGetFromPool(CanonBall pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(CanonBall pooledObject)
    {
        pooledObject?.gameObject.SetActive(false);        
    }

    private void OnDestroyPooledObject(CanonBall pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }



    private void Start()
    {
        mc = GetComponent<Move_Canon>();
        if (mc == null)
        {
            Debug.LogError("Move_Canon component is missing!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mc.isPlaying)
        {
            CheckForInput();
        }
        
    }

    private void CheckForInput()
    {
        // Check if there is at least one touch
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            if (Time.time > nextTimeToShoot && canonBallPool != null)
            {
                ShootCanonBall();
            }
        }
        // Check for mouse input (for WebGL and desktop platforms)
        else if (Input.GetMouseButton(0)) // Detect left mouse button click
        {
            // Use mouse position as touch position
            Vector2 mousePosition = Input.mousePosition;

            // Process the mouse input
            if (Time.time > nextTimeToShoot && canonBallPool != null)
            {
                ShootCanonBall();
            }
        }
    }

    private void ShootCanonBall()
    {
        // get a pooled object
        CanonBall CB_Object = canonBallPool.Get();

        if (CB_Object == null)
            return;

        // align the CB to Canon
        CB_Object.transform.SetPositionAndRotation(Canon_Transform.position, Canon_Transform.rotation);

        // move Cb forward
        CB_Object.GetComponent<Rigidbody2D>().AddForce(CB_Object.transform.up * CB_Velocity, ForceMode2D.Impulse);

        CB_Object.Deactivate();

        // set cooldown delay
        nextTimeToShoot = Time.time + Canon_CoolDownWindow;

    }


}
