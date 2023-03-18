using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    public float minRadius = 1.0f;      // Starting radius of impact effect
    public float maxRadius = 5.0f;      // Maximum radius of impact effect
    public float duration = 1.0f;       // Duration of impact effect
    public AnimationCurve curve;        // Curve to use for expanding the radius over time

    private float startTime;
    private float endTime;

    public float raycastDistance = 200f;
    public LayerMask raycastLayerMask;

    void Start()
    {
        startTime = Time.time;
        endTime = startTime + duration;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, raycastLayerMask))
        {
            // Move the GameObject to the position of the hit
            transform.position = hit.point;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;
        float radius = Mathf.Lerp(minRadius, maxRadius, curve.Evaluate(t));
        transform.localScale = new Vector3(radius, .1f, radius);

        if (Time.time >= endTime)
        {

            //Here we need to spawn the damaging effect capsule whatever

            Destroy(gameObject);
        }
    }
}