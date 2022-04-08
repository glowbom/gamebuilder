using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesAnimation : MonoBehaviour
{

	public GameObject note; 
	public GameObject guitarNeck; 
	public GameObject startPointNode; 
	public GameObject endPointNode; 


	// Reference (do we need movement speed variable?)
	// https://docs.unity3d.com/ScriptReference/Transform-position.html 

	// Path creation reference
	// https://forum.unity.com/threads/move-gameobject-along-a-given-path.455195/


    // Start is called before the first frame update
    void Start()
    {
    	// Start at start point
        note.gameObject.transform.position = startPointNode.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

       // note.gameObject.transform.Translate(0, 0, 0.02f); 

    	note.gameObject.transform.position = Vector3.MoveTowards(note.gameObject.transform.position, endPointNode.gameObject.transform.position, 0.02f); 
    }
}
