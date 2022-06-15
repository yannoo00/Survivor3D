using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox : MonoBehaviour,IItemDrop
{

    public int ammo = 200;
    public GameObject text;
    string content;
    void Start()
    {
        content = "Ammo +"+ammo;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {
        target.GetComponentInChildren<Gun>().ammoRemain += ammo;

        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;

        Destroy(gameObject);
    }
}
