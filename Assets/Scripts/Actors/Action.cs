using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void Move(Actor actor, Vector2 direction)
    {
        actor.Move(direction);
        actor.UpdateFieldOfView();
    }

    internal static void Hit(Actor actor1, Actor actor2)
    {
        throw new NotImplementedException();
    }

    internal static void MoveOrHit(Actor actor, Vector2 direction)
    {
        throw new NotImplementedException();
    }
}
