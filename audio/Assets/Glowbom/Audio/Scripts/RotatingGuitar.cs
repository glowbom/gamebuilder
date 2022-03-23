using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGuitar : MonoBehaviour
{

	//public float speed = 100;
    public GameObject guitar; 
    public GameObject rotatingPoint; 

    public float xSpeed = 0.0f;
 	public float ySpeed = 10.0f;
 	public float zSpeed = 0.0f;

 	public Vector3 rotationVector; 

 	public float speed; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //guitar.transform.Rotate(0, speed * Time.deltaTime, 0);

       //  transform.Rotate(
       //      xSpeed * Time.deltaTime,
       //      ySpeed * Time.deltaTime,
       //      zSpeed * Time.deltaTime
       // );

    	//transform.RotateAround(rotatingPoint.transform.position, rotationVector, speed);

    	transform.Rotate( new Vector3(0, 0.3f, 0) );
    }
}
