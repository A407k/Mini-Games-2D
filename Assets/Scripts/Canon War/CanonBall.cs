using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CanonBall : MonoBehaviour
{
    //deactivate after delay
    [SerializeField] private float timeoutDelay = 3f;

    private IObjectPool<CanonBall> objectPool;

    //public property to give the CB a reference to its ObjectPool
    public IObjectPool<CanonBall> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    IEnumerator DeactivateRoutine(float Delay)
    {
        yield return new WaitForSeconds(Delay);

        //reset the moving rigidbody
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();
        if (rBody != null)
        {
            rBody.velocity = Vector2.zero;       // Stop linear movement
            rBody.angularVelocity = 0f;         // Stop rotation
        }
        

        //release the CB back to pool
        objectPool?.Release(this);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BB"))
        {
            objectPool?.Release(this);
        }
    }


}
