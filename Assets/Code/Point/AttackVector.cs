using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVector : MonoBehaviour
{
    public float speed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
    }

    public void LookAtMouse()
    {
        Vector3 direction = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        Vector3 vectorToTarget = direction - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }
}
