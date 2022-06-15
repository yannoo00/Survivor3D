using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedGem : MonoBehaviour,IItemDrop
{

    public GameObject text;
    string content;

    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        content = "Speed +"+speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {
        target.GetComponent<PlayerMovement>().moveSpeed += speed;

        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;
    
        Destroy(gameObject);
    }
}
