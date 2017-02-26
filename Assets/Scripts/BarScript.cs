using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BarScript : MonoBehaviour {
    
    public enum ControllingPlayers { P1 = 1, P2 = 2 }

    [SerializeField]
    private ControllingPlayers ControllingPlayer = ControllingPlayers.P1;

    [SerializeField]
    private float MovementSpeed = 2f;
    [SerializeField]
    private float RotationSpeed = 20f;
    [SerializeField]
    private float WindUpAngle = 135f;
    [SerializeField]
    private float KickEndAngle = 135f;
    [SerializeField]
    private float MinKickSpeed = 80f;
    [SerializeField]
    private float MaxKickSpeed = 140f;
    [SerializeField]
    private float XLocalPositionLimit = 2f;

    private Rigidbody _attachedRigidbody;
    private Transform _attachedTransform;
    private string _verticalAxisString = "Vertical";
    private string _shootAxisString = "Shoot";
    private Quaternion _uprightRotation;
    private Quaternion _windUpRotation;
    private Quaternion _kickEndRotation;
    private float _axis;
    private float _previousAxis;
    private bool _kicking = false;

    private void Awake()
    {
        _attachedRigidbody = GetComponent<Rigidbody>();
        _attachedTransform = GetComponent<Transform>();

        _verticalAxisString += ((int)ControllingPlayer).ToString();
        _shootAxisString += ((int)ControllingPlayer).ToString();

        _attachedRigidbody.centerOfMass = Vector3.zero;
        _attachedRigidbody.maxAngularVelocity = MaxKickSpeed;
        _uprightRotation = _attachedRigidbody.rotation;
        _windUpRotation = _uprightRotation * Quaternion.Euler(0f, -WindUpAngle, 0f);
        _kickEndRotation = _uprightRotation * Quaternion.Euler(0f, KickEndAngle, 0f);

        _axis = Input.GetAxis(_shootAxisString);
        _previousAxis = Input.GetAxis(_shootAxisString);
    }

    private float _axisDelta;
    private Quaternion _rotationToMoveTowards;
    private float _rotationDirection = 1f;
    private void FixedUpdate ()
    {
        _attachedRigidbody.velocity = _attachedTransform.up * (-Input.GetAxis(_verticalAxisString) * MovementSpeed);

        if(!_kicking)
        {
            _axis = Input.GetAxis(_shootAxisString);
            if(_axis >= 0f)
            {
                _rotationToMoveTowards = Quaternion.Lerp(_uprightRotation, _windUpRotation, _axis);
            }
            else
            {
                _rotationToMoveTowards = Quaternion.Lerp(_uprightRotation, _kickEndRotation, Mathf.Abs(_axis));
            }

            _axisDelta = _previousAxis - _axis;
            if(_axisDelta > 0f)
            {
                _rotationDirection = 1f;
            }
            else if(_axisDelta < 0f)
            {
                _rotationDirection = -1f;
            }

            if(Quaternion.Angle(_attachedRigidbody.rotation, _rotationToMoveTowards) > RotationSpeed)
            {
                _attachedRigidbody.angularVelocity = _attachedTransform.up * (RotationSpeed * _rotationDirection);
            }
            else
            {
                _attachedRigidbody.angularVelocity = Vector3.zero;
                _attachedRigidbody.rotation = _rotationToMoveTowards;
            }
            //_attachedRigidbody.rotation = Quaternion.RotateTowards(_attachedRigidbody.rotation, _rotationToMoveTowards, RotationSpeed);
            //if (_axis >= _previousAxis)
            //{
            //    _attachedRigidbody.rotation = Quaternion.RotateTowards(_attachedRigidbody.rotation, _currentRotation, RotationSpeed);
            //}
            //else
            //{
            //    _axisDelta = _previousAxis - _axis;
            //    if (_axisDelta > 0.2f)
            //    {
            //        _kicking = true;
            //        StartCoroutine(Kick());
            //    }
            //    else
            //    {
            //        _attachedRigidbody.rotation = Quaternion.RotateTowards(_attachedRigidbody.rotation, _currentRotation, RotationSpeed);
            //    }
            //}

            _previousAxis = _axis;
        }
    }
    
    private IEnumerator Kick()
    {
        float kickSpeed = Mathf.Lerp(MinKickSpeed, MaxKickSpeed, _axis);
        Quaternion rotationIncrement = Quaternion.AngleAxis(kickSpeed, _attachedTransform.up);

        while(Quaternion.Angle(_attachedRigidbody.rotation, _kickEndRotation) > MaxKickSpeed + 1f)
        {
            _attachedRigidbody.rotation = rotationIncrement * _attachedRigidbody.rotation;
            yield return new WaitForFixedUpdate();
        }

        _attachedRigidbody.velocity = Vector3.zero;
        _attachedRigidbody.angularVelocity = Vector3.zero;

        float lerpValue = 0f;
        float timer = 0f;
        while (lerpValue < 1f)
        {
            timer += Time.fixedDeltaTime;
            lerpValue = timer / 0.1f;

            _attachedRigidbody.rotation = Quaternion.Lerp(_kickEndRotation, _uprightRotation, lerpValue);
            
            yield return new WaitForFixedUpdate();
        }

        _attachedRigidbody.velocity = Vector3.zero;
        _attachedRigidbody.angularVelocity = Vector3.zero;
        _attachedRigidbody.rotation = _uprightRotation;

       _kicking = false;

        _previousAxis = Mathf.Max(Input.GetAxis(_shootAxisString), 0f);
    }

    Vector3 _currentLocalPosition;
    private void LateUpdate()
    {
        _currentLocalPosition = _attachedTransform.localPosition;
        if (_currentLocalPosition.x > XLocalPositionLimit)
        {
            _currentLocalPosition.x = XLocalPositionLimit;
            _attachedTransform.localPosition = _currentLocalPosition;
        }
        else if (_currentLocalPosition.x < -XLocalPositionLimit)
        {
            _currentLocalPosition.x = -XLocalPositionLimit;
            _attachedTransform.localPosition = _currentLocalPosition;
        }
    }

    public float GetKickingForce()
    {
        return Mathf.Abs(Input.GetAxis(_shootAxisString)) * RotationSpeed;
    }
}
