using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    private Rigidbody _attachedRigidbody;
    private Transform _attachedTransform;

    private void Awake()
    {
        _attachedRigidbody = GetComponent<Rigidbody>();
        _attachedTransform = GetComponent<Transform>();
    }

    private Vector3 _shotDirection = Vector3.zero;
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _shotDirection.z = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _shotDirection.z = -1f;
        }
        else
        {
            _shotDirection.z = 0f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _shotDirection.x = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _shotDirection.x = -1f;
        }
        else
        {
            _shotDirection.x = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _attachedRigidbody.AddForce(_shotDirection * 3f, ForceMode.Impulse);
        }
    }

    private Vector3 _tableCenter = Vector3.zero;
    private void FixedUpdate()
    {
        _tableCenter.y = _attachedTransform.position.y;
        _attachedRigidbody.AddForce((_tableCenter - _attachedTransform.position).normalized * 0.08f, ForceMode.Acceleration);
    }

	private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Wall")
        {
            float bounceSpeed = Mathf.Max(_attachedRigidbody.velocity.magnitude, 3f);
            _attachedRigidbody.velocity = Vector3.Reflect((_attachedTransform.position - col.contacts[0].point).normalized * bounceSpeed, col.contacts[0].normal);
        }
        else if (col.gameObject.tag == "Floor")
        {
            _attachedRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
}
