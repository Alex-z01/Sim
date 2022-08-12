using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerInventory PlayerInventory;
    PlayerMovement PlayerMovement;
    Animator anim;

    private void Start()
    {
        PlayerInventory = GetComponent<PlayerInventory>();
        PlayerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    public void AnimControls()
    {
        if (PlayerMovement.movementInput.magnitude > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        anim.SetFloat("x", PlayerMovement.lastDir.normalized.x);
        anim.SetFloat("y", PlayerMovement.lastDir.normalized.y);

        foreach(EquippableSocket socket in PlayerInventory.sockets)
        {
            socket.SetMoveVector(PlayerMovement.movementInput, PlayerMovement.lastDir);
        }
    }
}
