using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class AutoAnimatorSetup : MonoBehaviour
{
    public RuntimeAnimatorController sharedController;
    public Sprite[] characterSprites;

    void Reset()
    {
        var animator = GetComponent<Animator>();
        if (sharedController != null) animator.runtimeAnimatorController = sharedController;

        var renderer = GetComponent<SpriteRenderer>();
        if (characterSprites != null && characterSprites.Length > 0)
            renderer.sprite = characterSprites[0]; // 기본 외형만 설정
    }
}