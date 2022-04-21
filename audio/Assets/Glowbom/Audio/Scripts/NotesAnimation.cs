using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesAnimation : MonoBehaviour
{

	//MARK: Notes
	public GameObject note; 
	public GameObject noteTwo; 
	public GameObject noteThree; 


	//MARK: Other properties
	public GameObject guitarNeck; 

	//MARK: Paths
	public GameObject startPointNode; 
	public GameObject endPointNode; 
	public GameObject startPointNodeTwo; 
	public GameObject endPointNodeTwo; 
	public GameObject startPointNodeThree; 
	public GameObject endPointNodeThree; 


	// Reference (do we need movement speed variable?)
	// https://docs.unity3d.com/ScriptReference/Transform-position.html 

	// Path creation reference
	// https://forum.unity.com/threads/move-gameobject-along-a-given-path.455195/


	//MARK: Detecting touches



    // Start is called before the first frame update
    void Start()
    {
    	// Start at start point
        note.gameObject.transform.position = startPointNode.gameObject.transform.position;
        noteTwo.gameObject.transform.position = startPointNodeTwo.gameObject.transform.position;
        noteThree.gameObject.transform.position = startPointNodeThree.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

       // note.gameObject.transform.Translate(0, 0, 0.02f); 

    	note.gameObject.transform.position = Vector3.MoveTowards(note.gameObject.transform.position, endPointNode.gameObject.transform.position, 0.03f); 
    	noteTwo.gameObject.transform.position = Vector3.MoveTowards(noteTwo.gameObject.transform.position, endPointNodeTwo.gameObject.transform.position, 0.03f); 
    	noteThree.gameObject.transform.position = Vector3.MoveTowards(noteThree.gameObject.transform.position, endPointNodeThree.gameObject.transform.position, 0.03f); 
    }
}
