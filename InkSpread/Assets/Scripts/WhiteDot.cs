using Cocuy;
using UnityEngine;

public class WhiteDot : MonoBehaviour
{
    private ParticlesAreaManipulator pam;
    private float DefaultRadius;
    private float DefaultStrength;
    public float RadiusMulti;
    public float StrengthMulti;
    [Range(0, 1)] public float AffectByAnim;

    void Awake()
    {
        pam = GetComponent<ParticlesAreaManipulator>();
        DefaultRadius = pam.m_radius;
        DefaultStrength = pam.m_strength;
    }

    private float tick = 0f;
    public float interval = 2f;

    void Update()
    {
        pam.m_radius = DefaultRadius * Mathf.Lerp(1, RadiusMulti, AffectByAnim);
        pam.m_strength = DefaultStrength * Mathf.Lerp(1, StrengthMulti, AffectByAnim);

        interval = Random.Range(1f, 3f);
        tick += Time.deltaTime;
        if (tick > interval)
        {
            float RotZ = Random.Range(-45, 135);
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + RotZ);
            tick = 0f;
            interval = Random.Range(1f, 3f);
        }
    }
}