using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerInventory _playerInventory;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _playerMovement = GetComponent<PlayerMovement>();
        _anim = GetComponent<Animator>();
    }

    public void AnimControls()
    {
        if (_playerMovement.MovementInput.magnitude > 0 && _playerMovement.CanMove)
        {
            _anim.SetBool("isWalking", true);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }

        _anim.SetFloat("x", _playerMovement.LastDir.normalized.x);
        _anim.SetFloat("y", _playerMovement.LastDir.normalized.y);

        foreach(EquippableSocket socket in _playerInventory.sockets)
        {
            socket.SetMoveVector(_playerMovement.MovementInput, _playerMovement.LastDir);
        }
    }
}
