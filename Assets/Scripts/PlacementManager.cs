using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public Material startingMaterial;
    public Material highlightMaterial;
    public bool isTeam;
    public int x;
    public int y;
    private void OnMouseOver()
    {
        if (isTeam)
        {
            GetComponent<MeshRenderer>().material = highlightMaterial;
            GameManager.i.setHighlightedPlacement(transform.position);
        }
    }

    private void OnMouseExit()
    {
        if (isTeam)
        {
            GetComponent<MeshRenderer>().material = startingMaterial;
            GameManager.i.setHighlightedPlacement(null);
        }
    }
}
