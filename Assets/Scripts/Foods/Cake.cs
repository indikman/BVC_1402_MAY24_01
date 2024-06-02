using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Cake : Food
{
    public Material cakeMat;
    public Color customColor = new Color(0, 0, 0, 0);
    public int timesChanged = 0;
    public bool canCollect = false;
    // Start is called before the first frame update
    void Start()
    {
        Value = GameConstants.CakeValue;
        cakeMat = this.GetComponent<MeshRenderer>().material;
        
        StartCoroutine(changeColor());
    }
    private IEnumerator changeColor()
    {
        yield return new WaitForSeconds(0.1f);
        if(customColor.a <= 1)
        {
            customColor.a += 0.02f;
            customColor.r += 0.02f;
            customColor.g += 0.02f;
            customColor.b += 0.02f;
            timesChanged++;
            StartCoroutine(changeColor());
        }
        if(customColor.a >=0.9f) 
        {
            canCollect = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        cakeMat.SetColor("_Color", customColor);
    }
}
