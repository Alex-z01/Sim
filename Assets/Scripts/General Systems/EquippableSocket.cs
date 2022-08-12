using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableSocket : MonoBehaviour
{
    public Animator myAnim { get; set; }
    public Animator playerAnim;

    private SpriteRenderer spriteRenderer;

    private AnimatorOverrideController animOverrideController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponentInParent<Animator>();
        myAnim = GetComponent<Animator>();

        animOverrideController = new AnimatorOverrideController(myAnim.runtimeAnimatorController);

        myAnim.runtimeAnimatorController = animOverrideController;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }

    public void SetMoveVector(Vector2 movementInput, Vector2 lastDir)
    {
        if (movementInput.magnitude > 0)
        {
            myAnim.SetBool("isWalking", true);
        }
        else
        {
            myAnim.SetBool("isWalking", false);
        }

        myAnim.SetFloat("x", lastDir.normalized.x);
        myAnim.SetFloat("y", lastDir.normalized.y);
    }

    public void Equip(AnimationClip[] clips)
    {
        spriteRenderer.color = Color.white;

        animOverrideController["IdleBack"] = clips[0];
        animOverrideController["IdleFront"] = clips[1];
        animOverrideController["IdleLeft"] = clips[2];
        animOverrideController["IdleRight"] = clips[3];

        animOverrideController["WalkDown"] = clips[4];
        animOverrideController["WalkLeft"] = clips[5];
        animOverrideController["WalkRight"] = clips[6];
        animOverrideController["WalkUp"] = clips[7];
    }

    public void UnEquip()
    {
        animOverrideController["IdleBack"] = null;
        animOverrideController["IdleFront"] = null;
        animOverrideController["IdleLeft"] = null;
        animOverrideController["IdleRight"] = null;

        animOverrideController["WalkDown"] = null;
        animOverrideController["WalkLeft"] = null;
        animOverrideController["WalkRight"] = null;
        animOverrideController["WalkUp"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
