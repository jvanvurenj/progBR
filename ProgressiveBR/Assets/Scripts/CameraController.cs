using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        

        offset = transform.position - player.transform.position;
        print(offset);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
    
    public void setTarget(GameObject target)
    {
        player = target;
        offset = transform.position - player.transform.position;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);

    }

   
}
