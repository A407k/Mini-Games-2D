using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class BouncingBall : MonoBehaviour
{

    private IObjectPool<BouncingBall> objectPool;

    // Reference property to give BB a reference to its own Pool
    public IObjectPool<BouncingBall> ObjectPool { set=> objectPool = value; }

    [SerializeField] private int life_Num; // Life Points of the Bouncing Ball

    public int starting_life;  // Store the starting life points of BB for Score

    private TextMeshProUGUI display_Life; // Text component to Display the life number on BB

    private Rigidbody2D rbody; // The rigidbody2D component of BB 

    [SerializeField]
    private float maxSpeed;

    private void Start()
    {
        // Find the rigidbody2D
        rbody = GetComponent<Rigidbody2D>();

        // Find the TMP component
        display_Life = GetComponentInChildren<TextMeshProUGUI>();
        if (display_Life == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing!");
            display_Life = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Set the intial display number
        UpdateDisplaynumber();
                
    }

    public void setBB_life(int bb_life)
    {
        
        life_Num = bb_life;

        starting_life = bb_life;
        
        UpdateDisplaynumber() ;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CB")) // Compare the tag
        {
            // Decrease the Life of BB
            life_Num--;

            // Update the diplay number
            UpdateDisplaynumber();

            if (life_Num <= 0)
            {
                returnTheBall();
            }

        }
    }

    private void returnTheBall()
    {
        // reset the moving body
        
        if (rbody != null)
        {
            rbody.velocity = Vector2.zero;
            rbody.angularVelocity = 0f;
        }

        // release the ball to Pool
        objectPool?.Release(this);

    }

    private void UpdateDisplaynumber()
    {
        if (display_Life != null)
        {
            display_Life.text = life_Num.ToString();
        }
    }


}
