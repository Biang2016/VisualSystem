using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Text Text;
    public Animator Anim;

    public void OnHover()
    {
        Anim.SetTrigger("Hide");
    }

    public void OnExit()
    {
        Anim.SetTrigger("Show");
    }
}