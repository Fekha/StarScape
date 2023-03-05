using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacement : MonoBehaviour
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
            GameManager.i.setHighlightedPlacement(this);
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
