using System;
using UnityEngine;

public class PlayerController
{
    private Player _player;

    public Action ArtificialFixedUpdate = delegate { };
    public Action ArtificialUpdate = delegate { };

    public PlayerController(Player player)
    {
        _player = player;

        ArtificialFixedUpdate += Moving;
        ArtificialFixedUpdate += Rotating;

        ArtificialUpdate += ThrowingGrenade;
        ArtificialUpdate += Shooting;
        ArtificialUpdate += Featching;
        ArtificialUpdate += ShowLastKill;
        ArtificialUpdate += Target;
        ArtificialUpdate += FreeTaregt;
    }

    private void Moving() => _player.Move(Input.GetAxis("Vertical"));
    private void Rotating() => _player.Rotate(Input.GetAxis("Horizontal"));

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
            _player.StartShooting();
       
        if (Input.GetMouseButtonUp(0))
            _player.StopShooting();
    }

    private void ThrowingGrenade()
    {
        if(Input.GetKey(KeyCode.Q)) _player.ThrowingGrenade();
    }
    private void Target()
    {
        if(Input.GetKeyDown(KeyCode.E)) _player.TargetSystem();
    }
    private void FreeTaregt()
    {
        if(Input.GetKey(KeyCode.C)) _player.ReleasTarget();
    }
    
    private void Featching()
    {
        if(Input.GetKey(KeyCode.I)) _player.Featch();
    }
    private void ShowLastKill() => _player.ShowLastEnemy(Input.GetKey(KeyCode.Tab));
    
}