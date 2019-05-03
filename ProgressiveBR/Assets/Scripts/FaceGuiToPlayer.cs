using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FaceGuiToPlayer : NetworkBehaviour
{

    private Camera thisCamera;
    public Canvas thisCanvas;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<PlayerMovement>().MyCamera;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        Vector3 targetPostition = new Vector3(thisCamera.transform.position.x, thisCanvas.transform.position.y, thisCamera.transform.position.z);
        thisCanvas.transform.LookAt(targetPostition);
      
    }
}
