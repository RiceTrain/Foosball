using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour {
    
    public GameObject BallPrefab;
    public float RandomLaunchAngle = 5f;
    public float LaunchSpeed = 5f;

    private Vector3 _originalForward;
    private Rigidbody _spawnedBallRigidbody;

    private void Start()
    {
        _originalForward = transform.forward;
    }

    private void Update()
    {
        if (Input.GetButtonDown("ResetBall"))
        {
            if(_spawnedBallRigidbody != null)
            {
                _spawnedBallRigidbody.velocity = Vector3.zero;
                _spawnedBallRigidbody.angularVelocity = Vector3.zero;
                _spawnedBallRigidbody.constraints = RigidbodyConstraints.None;
                _spawnedBallRigidbody.transform.position = transform.position;

                LaunchBall();
            }
        }
    }

    public void LaunchNewBall()
    {
        GameObject newBall = Instantiate(BallPrefab, transform.position, transform.rotation) as GameObject;
        _spawnedBallRigidbody = newBall.GetComponent<Rigidbody>();

        LaunchBall();
    }

    private void LaunchBall()
    {
        transform.Rotate(transform.up, Random.Range(-RandomLaunchAngle, RandomLaunchAngle));
        _spawnedBallRigidbody.AddForce(transform.forward * LaunchSpeed, ForceMode.Impulse);

        transform.forward = _originalForward;
    }
}
