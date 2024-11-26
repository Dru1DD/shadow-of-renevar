using MyGame.PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ObjectInspector : MonoBehaviour
{
    private GameObject _inspectableObject;
    [SerializeField] private CinemachineVirtualCamera _cvc;

    [Header("Button Setup")]
    [SerializeField] private KeyCode _openInspectionButton = KeyCode.F;
    [SerializeField] private KeyCode _closeInspectionButton = KeyCode.P;

    [Header("Pick up Settings")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _reachDistance = 1.5f;

    [SerializeField] private InspectionCamera _inspectionCamera;

    [Header("UI Elements")]
    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject _inspectionCanvas;
    [SerializeField] private GameObject _mainCanvas;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] sounds;
    
    private AudioSource audioSource;

    private bool isInspecting = false;
    private FirstPlayerController firstPersonController;

    private void Start()
    {
        if (hintText != null)
            hintText.SetActive(false);

        firstPersonController = FindObjectOfType<FirstPlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInspecting)
        {
            if (Input.GetKeyDown(_closeInspectionButton))
            {
                ExitInspectionMode();
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                RotateLeft();
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                RotateRight();
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                RotateUp();
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                RotateDown();
            }


            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _reachDistance))
        {
            if (hit.collider.gameObject.GetComponent<InspectableObject>() != null)
            {
                if (hintText != null)
                    hintText.SetActive(true);

                if (Input.GetKeyDown(_openInspectionButton))
                {
                    StartInspection(hit.collider.gameObject);
                }
                return;
            }
        }

        if (hintText != null)
            hintText.SetActive(false);
    }

    private void StartInspection(GameObject objectToInspect)
    {
        PlaySound(audioSource, sounds[0]);

        _inspectableObject = Instantiate(objectToInspect, _inspectionCamera.transform.GetChild(0));
        _inspectableObject.GetComponent<Rigidbody>().isKinematic = true;

        InspectableObject inspectableObject = _inspectableObject.GetComponent<InspectableObject>();
        inspectableObject.transform.localPosition = Vector3.zero + inspectableObject.spawnPositionOffset;
        inspectableObject.transform.localRotation = Quaternion.Euler(Vector3.zero + inspectableObject.spawnRotationOffset);

        _inspectionCanvas.SetActive(true);
        _inspectionCamera.inspectableObject = inspectableObject;
        _inspectionCamera.gameObject.SetActive(true);

        Destroy(objectToInspect);

        _mainCanvas.SetActive(false);
        TurnOffCameraMovement();

        if (firstPersonController != null) firstPersonController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isInspecting = true;

        if (hintText != null)
            hintText.SetActive(false);
    }

    private void ExitInspectionMode()
    {
        Destroy(_inspectableObject);
        _inspectionCanvas.SetActive(false);
        _inspectionCamera.gameObject.SetActive(false);
        _mainCanvas.SetActive(true);
        TurnOnCameraMovement();

        if (firstPersonController != null) firstPersonController.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isInspecting = false;
    }

    private void TurnOnCameraMovement()
    {
        if (_cvc == null)
        {
            Debug.LogWarning("Cinemachine Virtual Camera (_cvc) not exist. Null.");
            return;
        }

        CinemachinePOV pov = _cvc.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
            pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            Debug.LogWarning("CinemachinePOV component was not found at _cvc.");
        }
    }

    private void TurnOffCameraMovement()
    {
        if (_cvc == null)
        {
            Debug.LogWarning("Cinemachine Virtual Camera (_cvc) not exist. Null.");
            return;
        }

        CinemachinePOV pov = _cvc.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            pov.m_HorizontalAxis.m_InputAxisName = "";
            pov.m_VerticalAxis.m_InputAxisName = "";
            pov.m_HorizontalAxis.m_InputAxisValue = 0;
            pov.m_VerticalAxis.m_InputAxisValue = 0;
        }
        else
        {
            Debug.LogWarning("CinemachinePOV component was not found at _cvc.");
        }
    }

    public void RotateLeft()
    {
        if (_inspectableObject != null)
            _inspectableObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        if (_inspectableObject != null)
            _inspectableObject.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
    }

    public void RotateUp()
    {
        if (_inspectableObject != null)
            _inspectableObject.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }

    public void RotateDown()
    {
        if (_inspectableObject != null)
            _inspectableObject.transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
    }

    public void PlaySound (AudioSource audioSource, AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}
