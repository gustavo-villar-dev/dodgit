using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float speed = 8f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime; 

        if (transform.position.y <= -10f){
            Destroy(gameObject);
        }
    }
}
