using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DiceStat : MonoBehaviour
{
    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
	float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to control throw force in Z direction
    
    [SerializeField] Transform[] diceSides;

    public int side = 0;
    int sideTotal;
    Text totalDisplay;
    Canvas canvas;
    DiceTotal total;

    public Camera mainCamera;

    Rigidbody rb;
    
    bool hasLanded;
    bool thrown;

    Vector3 initPos;

    float x;
    float y;
    float z;

    public Button reset;

    //Array of potential dice start positions
    public List<Vector3> positionArray = new List<Vector3>(){
    new Vector3(-3.0f, 2.0f, 0.0f), new Vector3(-3.0f, 2.0f, -2.0f),
    new Vector3(-3.0f, 2.0f, 2.0f), new Vector3(-3.0f, 4.0f, 0.0f),
    new Vector3(-3.0f, 4.0f, -2.0f), new Vector3(-3.0f, 4.0f, 2.0f),
    new Vector3(-3.0f, 2.0f, -4.0f), new Vector3(-3.0f, 2.0f, 4.0f),
    new Vector3(-3.0f, 4.0f, -4.0f), new Vector3(-3.0f, 4.0f, 4.0f)
    };

    void RandomiseRotation()
    {
        var euler = transform.eulerAngles;
        euler.x = Random.Range(0.0f, 360.0f);
        euler.y = Random.Range(0.0f, 360.0f);
        euler.z = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = euler;
    }

    void Start()
    {   
        sideTotal = 0;
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        RandomiseRotation();

        Button resetButton = GameObject.FindGameObjectWithTag("resetButton").GetComponent<Button>();
		resetButton.onClick.AddListener(() => Reset());

        totalDisplay = GameObject.FindGameObjectWithTag("DiceTotal").GetComponent<Text>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }
    
    void Update()
    {
        // if you touch the screen
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {

			// getting touch position and marking time when you touch the screen
			touchTimeStart = Time.time;
			startPos = Input.GetTouch (0).position;
		}

		// if you release your finger
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
			// marking time when you release it
			touchTimeFinish = Time.time;

			// calculate swipe time interval 
			timeInterval = touchTimeFinish - touchTimeStart;

			// getting release finger position
			endPos = Input.GetTouch (0).position;

			// calculating swipe direction in 2D space
			direction = endPos - startPos;
            x = direction[1];
            y = direction[0];

			// add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
			rb.isKinematic = false;
            RollDice();
            
        }

        //Checks dice side value if the dice is stationary and has been thrown
        if(rb.IsSleeping() && !hasLanded && thrown){
            hasLanded = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            CheckDiceSide();
        
        }
        else if(rb.IsSleeping() && hasLanded && side == 0){
            Debug.Log("Dice did not land on a side.");
            RollAgain();
        }
    }
    public void RollDice(){
        Vector3 newDist = new Vector3(0.3f * x, 0, -0.3f * y);

        if(!thrown && !hasLanded && ((Mathf.Abs(direction[0])+ Mathf.Abs(direction[1])) > 100.0f)){
            thrown = true;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(newDist / timeInterval);
        }

        else if(thrown && hasLanded){
            Debug.Log("Still rolling");
        }
    }

    private int MaxValue(double[] arr){
        double max = 0.0;
        int location = 0;

        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] > max)
            {
                max = arr[i];
                location = i;
            }
        }
        return location;
    }
    
    public void Reset(){

        float radius = 0.6f;
        int i = 0;
        bool moved = false;

        totalDisplay.text = "";
        mainCamera.GetComponent<Zoom>().ResetCamera();
        canvas.GetComponent<DiceTotal>().resetTotal();

        if(!(Physics.CheckSphere (initPos, radius))){
            transform.position = initPos;

        }else{
            while(i < positionArray.Count && !moved){

                if (!(Physics.CheckSphere (positionArray[i], radius))) {
                    
                    Debug.Log("Spawn point found");
                    transform.position = positionArray[i];
                    moved = true;

                } else {
                    i++;
                }
            }
        }

        RandomiseRotation();

        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        sideTotal = 0;
    }

    void RollAgain(){
        Debug.Log("Re-rolling.");
        Reset();
        thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500),Random.Range(0,500), Random.Range(0,500));
    }

    void CheckDiceSide()
    {   
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        double[] yPositions = new double[diceSides.Length];

        for(int i = 0; i < diceSides.Length; i++)
        {
            yPositions[i] = diceSides[i].position.y;
        }

        side = MaxValue(yPositions) + 1;
        Debug.Log("Dice landed on " + side);

        canvas.GetComponent<DiceTotal>().setTotal(side);
        mainCamera.GetComponent<Zoom>().CenterCamera();
    }
    
    
}