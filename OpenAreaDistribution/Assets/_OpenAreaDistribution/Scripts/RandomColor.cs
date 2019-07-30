using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{

	void Start ()
    {
        var rand = Random.Range(0.05f,0.15f);
        transform.localScale = new Vector3(rand, rand, rand);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        var rand2 = Random.Range(0.15f, 0.37f);
        sr.color = new Color(rand2, rand2, rand2);
	}

}
