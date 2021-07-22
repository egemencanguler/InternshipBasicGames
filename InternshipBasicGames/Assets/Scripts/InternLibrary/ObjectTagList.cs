using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectTagList
{
    // TODO bu ne?
    // kullanildigi nerdeyse heryerde zaten objelerin turleri biliniyor

    /*
     *      buna benzer anlamsiz bir kod
     * 
     *       void Update
     *         var player = GetCollidedPlayer();
     *         gameCore.KillPlayer(player.type);
     * 
     *       void KillPlayer(PlayerTyep playerType)
     *         if playertype == Player1
     *             players[0].Kill()
     *         else playerType == Player2
     *             players[1].Kill();
     *
     *     aslinda yazilmasi gereken
     *     void Update
     *         GetCollidedPlayer().Kill();
     * 
     */
    
    
    
    public enum ObjectTags { Untagged,Wall, Player, Ball, LeftScore, RightScore,Alien,Food }

}


