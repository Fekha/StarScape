using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public Material myMaterial;
    public Material highlight;
    // Start is called before the first frame update
    private void Start()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
    }
    private void OnMouseOver()
    {
        myMaterial = highlight;
    }

    private void OnMouseExit()
    {
        
    }
}
