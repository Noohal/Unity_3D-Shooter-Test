using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JettSmoke : MonoBehaviour
{
    [SerializeField] GameObject smokeBall;

    public bool isControlled;

    private float smokeDuration = 3f;
    private float particleMovementSpeed = 30.0f;
    private float maxDistance = 50f;
    private float distanceTraveled = 0f;
    private float downwardForce = -2.0f;
    private float downwardForceIncrement = -3.8f;

    private Vector3 startingPosition;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isControlled)
        {
            transform.rotation = cam.transform.rotation;
        }

        Vector3 movement = (transform.forward * particleMovementSpeed * Time.deltaTime);
        if(!isControlled)
        {
            downwardForce += downwardForceIncrement * Time.deltaTime;
            movement += (transform.up * downwardForce * Time.deltaTime);
        }

        Vector3 newPos = transform.position + movement;
        distanceTraveled = Vector3.Distance(startingPosition, newPos);
        if(distanceTraveled > maxDistance)
        {
            OnCreateSmokeBall(transform.position);
        } else
        {
            transform.position += movement;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != 7)
        {
            OnCreateSmokeBall(collision.contacts[0].point);
        }
    }

    public void InitializeValues(bool control, Camera camera)
    {
        isControlled = control;
        cam = camera;
    }

    public void SetControl(bool control)
    {
        isControlled = control;
    }

    void OnCreateSmokeBall(Vector3 pos)
    {
        GameObject smokeBallObj = Instantiate(smokeBall, pos, transform.rotation);
        Destroy(this.gameObject);
        Destroy(smokeBallObj, smokeDuration);
    }

    IEnumerator destroySmoke()
    {
        yield return new WaitForSeconds(smokeDuration);
        Destroy(this.gameObject);
    }
}
