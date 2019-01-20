using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcdaRotate : MonoBehaviour {

    public float rot = 0.1f;
	
	void Update () {
        transform.Rotate(0f, 0f, rot);
	}
}
