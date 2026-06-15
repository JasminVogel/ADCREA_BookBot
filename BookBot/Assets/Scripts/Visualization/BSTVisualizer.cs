using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BSTVisualizer : MonoBehaviour
{
    public GameObject hologramPrefab; 
    public float flySpeed = 15f;
    public float waitTimeAtShelf = 0.2f;

    public float spawnHeight = 2f;
    public float shelfHoverHeight = 3f;

    private Action onAnimationComplete;

    public void PlayHologramAnimation(Vector3 startPos, List<Transform> shelvesToCheck, Action resumeRobotLogic)
    {
        this.onAnimationComplete = resumeRobotLogic;
        StartCoroutine(AnimateHologram(startPos, shelvesToCheck));
    }

    private IEnumerator AnimateHologram(Vector3 startPos, List<Transform> shelves)
    {
        if (shelves == null || shelves.Count == 0 || hologramPrefab == null)
        {
            if (onAnimationComplete != null)
            {
                onAnimationComplete();
            }
            yield break;
        }

        
        GameObject hologram = Instantiate(hologramPrefab, startPos + (Vector3.up * spawnHeight), Quaternion.identity);

        
        foreach (Transform targetShelf in shelves)
        {
            Vector3 targetPos = targetShelf.position + (Vector3.up * shelfHoverHeight); 

            while (Vector3.Distance(hologram.transform.position, targetPos) > 0.1f)
            {
                hologram.transform.position = Vector3.MoveTowards(hologram.transform.position, targetPos, flySpeed * Time.deltaTime);
                yield return null; 
            }


            yield return new WaitForSeconds(waitTimeAtShelf);
        }

       
        Renderer rend = hologram.GetComponent<Renderer>();
        if (rend != null) 
        {
        rend.material.color = Color.green;
        }

        
        yield return new WaitForSeconds(0.5f);
        Destroy(hologram);

        if (onAnimationComplete != null)
        {
            onAnimationComplete();
        }
    }
}
