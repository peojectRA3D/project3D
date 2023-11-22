using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public ParticleSystem boom;
    public Collider col;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void Awake()
    {
        boom.GetComponent<bulletStatus>().Damage = 2;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "booms")
        {
            boom.Play();
            StartCoroutine(endparticalboom(2f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator endparticalboom(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);

    }
    IEnumerator startboom(float delay)
    {
        yield return new WaitForSeconds(delay);

      
    }
}
