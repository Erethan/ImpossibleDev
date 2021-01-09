using UnityEngine;

public class FirstPersonCamera : MonoBehaviour 
{

    // Suppressing compiler warning "this variable is never used". Only used in the CustomEditor, only in Editor
#pragma warning disable CS0649
    [SerializeField] private Transform character;


    public static float xSensitivity = 1.2f;
    public static float ySensitivity = 1.2f;
    public static bool invertX = false;
    public static bool invertY = false;
    public bool clampVerticalRotation = true;
    public float minimumX = -90F;
    public float maximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;

#pragma warning restore CS0649

    private Quaternion m_characterTargetRot;
    private Quaternion m_cameraTargetRot;

    private bool cursorLock;

	void Start () 
    {
        ResetRotationTargets();
        ToggleCursorLock();
    }
	
    void Update()
    {
        float xRot = Input.GetAxis("Mouse Y") * xSensitivity * (invertY?-1:1);
        float yRot = Input.GetAxis("Mouse X") * ySensitivity * (invertX?-1:1);


        m_characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
        {
            m_cameraTargetRot = ClampRotationAroundXAxis(m_cameraTargetRot);
        }

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_characterTargetRot, smoothTime * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_cameraTargetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_characterTargetRot;
            transform.localRotation = m_cameraTargetRot;
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
    }

    public void ResetRotationTargets()
    {
        
        m_characterTargetRot = transform.parent.localRotation;
        m_cameraTargetRot = transform.localRotation;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion rot)
    {
        rot.x /= rot.w;
        rot.y /= rot.w;
        rot.z /= rot.w;
        rot.w = 1f;

        float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(rot.x);
        angleX = Mathf.Clamp(angleX, minimumX, maximumX);
        rot.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return rot;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ToggleCursorLock()
    {
        cursorLock = !cursorLock;
        Cursor.visible = !cursorLock;
        Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
}




