using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

//script written  by following along to tutorial videos of Valem https://www.youtube.com/watch?v=kfA_73npjMA
public class MovementRecogniser : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreashold = 0.1f;
    public Transform movementSource;

    public float newPositionThresholdDistance = 0.05f;// to only add a new position if it exceeds a threashold to not add too many.
    public GameObject debugCubePrefab;
    public bool creationMode = true; //the mode for adding gestures to the library.
    public string newGestureName;

    public float recognitionThreshold = 0.9f;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognised; 

    private List<Gesture> trainingSet = new List<Gesture>();
    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }

    }

    // Update is called once per frame
    void Update()
    { //check if players is pressing on the input button from the input source
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreashold);
        if(!isMoving && isPressed)
        {
            // starting the movement
            StartMovement();
        }

        else if (isMoving && !isPressed)
        {
            // ending the movement
            EndMovement();
        }

        else if (isMoving & isPressed)
        {
            // updating movement
            UpdateMovement();
        }
    
    }

    void StartMovement()
    {
        Debug.Log("Start Movement");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);
        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3); //instantiate and destroy after 3 seconds.
        }
    }

    void EndMovement()
    {
        Debug.Log("End Movement");
        isMoving = false;

        //Create Gesture from the Position List
        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count;  i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);
        //add a new gesture to training set
        if(creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);
            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml" ;
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);

        }
        else //recognise
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + result.Score);
            if(result.Score > recognitionThreshold)
            {
                OnRecognised.Invoke(result.GestureClass);
            }
        }


    }

    void UpdateMovement()
    {
        Debug.Log("Update Movement");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3); //instantiate and destroy after 3 seconds.
            }
            
        }
       
    }

}
