using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Para la rotacion de objetos
public class Rotate : MonoBehaviour
{
    private float speed = 0.01f;

    private void Update()
    {
        transform.Rotate(0, 0, 360 * speed * Time.deltaTime);
    }
}
