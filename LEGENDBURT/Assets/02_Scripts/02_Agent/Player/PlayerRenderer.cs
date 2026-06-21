using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerRenderer : MonoBehaviour, IModule
{
    private Animator m_Animator;
    public void Initialize(ModuleOwner owner)
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetBool(string boolName, bool value)
    {
        m_Animator.SetBool(boolName, value);
    }
}