using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class GoToHeaven : MonoBehaviour
{
    public GameObject background;
    public float x;
    public GameObject obj;
    private Transform _transform;
    private Transform _backgroundTransform;
    public float speedMultiplier = 500f; // Adjusted speed multiplier for smoother animation

    // Start is called before the first frame update
    void Start()
    {
        x = 250;
        background = GameObject.Find("Background");

        if (background != null)
        {
            _backgroundTransform = background.transform;
            _transform = transform;

            _transform.SetParent(_backgroundTransform);
            _transform.localScale = new Vector3(1.5f, 1.5f, 1);
            StartCoroutine(Die());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only move if the background is set
        if (_backgroundTransform != null)
        {
            _transform.position = new Vector3(_transform.position.x, x += speedMultiplier * Time.deltaTime, _transform.position.z);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        Destroy(obj);
    }
}
