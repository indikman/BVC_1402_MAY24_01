using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

public class Food : MonoBehaviour
{
    #region for respawnFood


    private bool isGrabbable = false;
    private GameObject foodPerfab;
    private Vector3 originalPosition;
    // private Coroutine foodCoroutine;
    private Renderer[] childRender;
    #endregion
    [SerializeField] private float respawnDuration =5f;
   // private Material foodMaterial;
    bool _hit = false;
    void Start()
    {
       // respawnDuration=5f;
        originalPosition = transform.position;
        childRender = GetComponentsInChildren<Renderer>();
        if (childRender != null )
        {
            Debug.Log("get render");
        }
        
    }

    IEnumerator RespawnCoroutine(Collider other,int respawnDuration)
    {
        

            Debug.Log("while");

           
            yield return new WaitForSeconds(respawnDuration);
           
            
            gameObject.SetActive(true);
            
    }
        
    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }
    void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(Value);
            
            StartCoroutine(RespawnCoroutine(other, 2));
            gameObject.SetActive(false);
            //isGrabbable = false;
           
        }
    }
}
