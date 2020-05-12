using UnityEngine;
using System.Collections;
using Cocuy;

public class EraserFluid : MonoBehaviour
{
    public ParticlesArea ParticlesArea;
    public Animator Anim;

    public float Dissipation;

    public void Clear()
    {
        Anim.SetTrigger("Clear");
    }

    public void Show()
    {
        Anim.SetTrigger("Show");
    }

    void Update()
    {
        ParticlesArea.Dissipation = Dissipation;
    }
}