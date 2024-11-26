using UnityEngine;

namespace MyGame.ObjectRotator {
    public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;

    private GameObject _objectToRotate;


    public void SetObjectToRotate(GameObject objectToRotate)
    {
        _objectToRotate = objectToRotate;
    }


    public void RotateLeft()
    {
        if (_objectToRotate != null)
            _objectToRotate.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        if (_objectToRotate != null)
            _objectToRotate.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
    }

    public void RotateUp()
    {
        if (_objectToRotate != null)
            _objectToRotate.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }

    public void RotateDown()
    {
        if (_objectToRotate != null)
            _objectToRotate.transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
    }
}

}
