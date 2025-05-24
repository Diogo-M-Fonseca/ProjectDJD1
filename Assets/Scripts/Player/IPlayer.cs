using UnityEngine;

public interface IPlayer
{ 
    float Speed { get; }
    float JumpForce { get; }
    float MaxJumpTime { get; }
    float JumpTime { get; set; }
}
