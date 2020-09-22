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

    //the value of the side that the dice landed on
    public int side = 0;
    //the total of all the dice values
    int sideTotal;

    //initialising the display of the total
    Text totalDisplay;
    Canvas canvas;
    DiceTotal total;

    bool stationary;
    GameObject[] diceList;

    public Camera mainCamera;

    Rigidbody rb;
    
    //stores the state of the dice
    bool hasLanded;
    bool thrown;

    Vector3 initPos;

    //co-ordinates
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

    //randomises the orientation of the dice so that the roll is fair
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
        //Stores the inital position of the dice
        initPos = transform.position;

        //disables gravity and freezes the dice ready for throwing
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        RandomiseRotation();

        //initialises the reset button
        Button resetButton = GameObject.FindGameObjectWithTag("resetButton").GetComponent<Button>();
		resetButton.onClick.AddListener(() => Reset());

        //initialises the total
        totalDisplay = GameObject.FindGameObjectWithTag("DiceTotal").GetComponent<Text>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }
    
    void Update()
    {
        //Finds the dice and adds them to a list
        diceList = GameObject.FindGameObjectsWithTag("d20");

        stationary = true;

        foreach(GameObject dice in diceList)
        {
            if(!dice.GetComponent<Rigidbody>().IsSleeping()){
                stationary = false;
            }
        }

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
    
    //returns dice to the start position
    public void Reset(){

        float radius = 0.6f;
        int i = 0;
        bool moved = false;
        canvas.GetComponent<Move>().StopMoving();

        totalDisplay.text = "";
        mainCamera.GetComponent<Zoom>().ResetCamera();
        canvas.GetComponent<DiceTotal>().resetTotal();

        //Re-enables collision
        foreach(GameObject dice in diceList){
            Rigidbody body = dice.GetComponent<Rigidbody>();
            body.detectCollisions = true;
        }

        if(!(Physics.CheckSphere (initPos, radius))){
            transform.position = initPos;

        }else{
            while(i < positionArray.Count && !moved){

                //checks if there is already a dice in that location
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

        //resets booleans and freezes dice
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        //resets dice total
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
        //freezes dice before counting
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
        

        // Moves dice and camera if all are stationary
        if(stationary)
        {
            Debug.Log("Positioning Dice");
            canvas.GetComponent<Move>().PositionDice();

            mainCamera.GetComponent<Zoom>().CenterCamera();
            canvas.GetComponent<DiceTotal>().showTotal();
        }
    }
    
    
}