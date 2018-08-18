using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    // Use this for initialization

 float movementFactor; // 0 for not mwoved, 1 for fully moved.

    Vector3 startingPos;
	void Start ()
    {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //set  movement factor

        //todo protect against  period is zero
        float cycles;

        if(period <= Mathf.Epsilon)
        {
            
                return;
        }
        else
        {
            cycles = Time.time / period;
        }
         //cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;
        //print(rawSinWave);


        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
        
	}
}
