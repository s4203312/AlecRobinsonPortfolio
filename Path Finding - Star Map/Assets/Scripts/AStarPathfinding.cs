using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinding
{
    private static List<Star> galaxyStarList;

    //Used to set the star list of the galaxy
    public static void SetGalaxyStarList(List<Star> stars) {
        galaxyStarList = new List<Star>(stars);
    }


}
