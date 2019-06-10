using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public Camera thisCamera;
    // Start is called before the first frame update

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null) { transform.position = player.transform.position + offset; }
    }
    
    public void setTarget(GameObject target)
    {
        player = target;
        offset = transform.position - target.transform.position;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offset.y + 3, target.transform.position.z -11 );

    }
    public void setSpectate(GameObject target){
        player = target;
        //offset.x = offset.x - target.transform.position.x;
        //offset.z = offset.z - target.transform.position.z;

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offset.y + 3, target.transform.position.z -11 );
        offset = transform.position - target.transform.position;
        //offset = new Vector3()
    }

   
}
