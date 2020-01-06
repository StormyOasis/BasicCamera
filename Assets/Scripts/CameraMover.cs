using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Rate at which camera changes look at position")]
    [Range(-100f, 100f)]
    public float LookSpeed = 0.5f;
  
    [SerializeField]
    [Range(0, 20)]
    private float zoomSpeed = 10;

    [SerializeField]
    private float minY = 0.01f;

    [SerializeField]
    private float maxY = 10;

    [SerializeField]
    [Range(0, 1f)]
    private float accelerationRate = 1;

    [Header("Clamping")]

    [SerializeField]
    private bool enableHorizontal = false;
    
    [SerializeField]
    [Tooltip("Min Rotation")]
    private float clampHorizMin = 0;

    [SerializeField]
    [Tooltip("Max Rotation")]
    private float clampHorizMax = 0;

    [Space]

    [SerializeField]
    private bool enableVertical = false;

    [SerializeField]
    [Tooltip("Min Rotation")]
    private float clampVerticalMin = 0;

    [SerializeField]
    [Tooltip("Max Rotation")]
    private float clampVerticalMax = 0;


    //Hidden fields
    private Vector3 currentLookAt;
    private float currentXAcceleration = 0;
    private float currentZAcceleration = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Adjust the camera look at value
        cameraLook();

        //Adjusting the zoom levels
        adjustZoom();

        //Perform keyboard navigation
        moveCamera();
    }

    void cameraLook()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        float speed = LookSpeed * Time.deltaTime;

        currentLookAt.x += (x * speed);
        currentLookAt.y += (y * speed);

        if (enableHorizontal)
        {
            if (currentLookAt.x > clampHorizMax)
                currentLookAt.x = clampHorizMax;
            if (currentLookAt.x < clampHorizMin)
                currentLookAt.x = clampHorizMin;
        }

        if (enableVertical)
        {
            if (currentLookAt.y > clampVerticalMax)
                currentLookAt.y = clampVerticalMax;
            if (currentLookAt.y < clampVerticalMin)
                currentLookAt.y = clampVerticalMin;
        }

        // X and Y axis seem to be swapped, so adjust accordingly
        transform.rotation = Quaternion.Euler(currentLookAt.y, currentLookAt.x, 0);
    }

    void adjustZoom()
    {
        float val = Input.GetAxis("Mouse ScrollWheel");
        
        if (val != 0)
        {
            Vector3 newPos = transform.position;
            newPos += transform.forward * (val * zoomSpeed);

            //Ensuring that we don't zoom through the ground or too far into the sky
            if (newPos.y >= minY && newPos.y < maxY)
            {
                transform.position = newPos;
            }
        }            
    }

    void moveCamera()
    {
        float time = Time.deltaTime;
        bool zPressed = false;
        bool xPressed = false;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentZAcceleration += accelerationRate * time;
            Vector3 newPos = transform.position;
            newPos.z += currentZAcceleration;
            transform.position = newPos;
            zPressed = true;
        }
        
        if(Input.GetKey(KeyCode.DownArrow))
        {
            currentZAcceleration += accelerationRate * time;
            Vector3 newPos = transform.position;
            newPos.z -= currentZAcceleration;
            transform.position = newPos;
            zPressed = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentXAcceleration += accelerationRate * time;
            Vector3 newPos = transform.position;
            newPos.x += currentXAcceleration;
            transform.position = newPos;
            xPressed = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentXAcceleration += accelerationRate * time;
            Vector3 newPos = transform.position;
            newPos.x -= currentXAcceleration;
            transform.position = newPos;
            xPressed = true;
        }

        if (!zPressed)
            currentZAcceleration = 0;

        if (!xPressed)
            currentXAcceleration = 0;
    }
}
