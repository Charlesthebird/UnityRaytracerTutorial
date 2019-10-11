using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    Transform groundLeft;
    Transform groundRight;
    public float speed = 2.0f;
    public float jumpForce = 10.0f;
    public float gravity = -9.8f;


    Vector3 velocity;
    Dictionary<string, Vector3> forces;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        velocity = Vector3.zero;
        forces = new Dictionary<string, Vector3>();

        sr = GetComponentInChildren<SpriteRenderer>();
        groundLeft = transform.Find("GroundLeft");
        groundRight = transform.Find("GroundRight");
    }
	
	// Update is called once per frame
	void Update () {


        var origin = (groundLeft.position + groundRight.position) / 2.0f;
        var size = new Vector2((groundRight.position - groundLeft.position).x, .03f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, 
            size, 180, Vector2.down,0);


        if(hit.transform != null)
        {
            // GROUNDED
            sr.material.color = Color.green;
            Vector3 g = Vector3.zero;
            forces.TryGetValue("gravity", out g);
            forces["gravityNormal"] = -g + (new Vector3(0,
                Mathf.Max(-velocity.y / Time.deltaTime, 0)));

            //if (Input.GetKeyDown(KeyCode.Space))
            if (Input.GetKey(KeyCode.Space))
            {
                forces["jump"] = new Vector3(0, jumpForce);
            }
            else
            {
                forces["jump"] = Vector3.zero;
            }
        }
        else
        {
            // NOT GROUNDED
            sr.material.color = Color.red;
            forces["gravityNormal"] = Vector3.zero;
            forces["gravity"] = new Vector3(0, gravity, 0);
            forces["jump"] = Vector3.zero;
        }
        var forceSum = Vector3.zero;
        foreach(var i in forces.Values)
        {
            Debug.Log("adding forces " + i);
            forceSum += i;
        }
            Debug.Log("\tsum " + forceSum);
        velocity = velocity + forceSum * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);



        Vector3 pIn = new Vector3();
        if (Input.GetKey(KeyCode.W)) pIn.y = speed;
        if (Input.GetKey(KeyCode.S)) pIn.y = -speed;
        if (Input.GetKey(KeyCode.D)) pIn.x = speed;
        if (Input.GetKey(KeyCode.A)) pIn.x = -speed;
        transform.Translate(pIn * Time.deltaTime);
    }
}
