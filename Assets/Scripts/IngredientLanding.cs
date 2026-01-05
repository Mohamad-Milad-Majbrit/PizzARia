using UnityEngine;

public class IngredientLanding : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        rb.constraints = RigidbodyConstraints.None;

        rb.linearDamping = 5f;   
        rb.angularDamping = 10f;

        hasLanded = true;
    }
}