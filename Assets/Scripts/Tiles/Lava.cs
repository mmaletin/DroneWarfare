
using UnityEngine;

public class Lava : MonoBehaviour, ITile
{
    public void Activate(PlayerRobot playerRobot)
    {
        playerRobot.overLava = true;
    }

    public void Deactivate(PlayerRobot playerRobot)
    {
        playerRobot.overLava = false;
    }
}