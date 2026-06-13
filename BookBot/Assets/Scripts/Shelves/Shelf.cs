using UnityEngine;
using System.Collections.Generic;

public class Shelf : MonoBehaviour
{
    public Color lightColor = new Color(1f, 0.5f, 0.5f);
    public Color darkColor = new Color(0.4f, 0f, 0f);  
    public int booksPerRow = 8;


    public float spacing = 0.5f; 
    public Vector3 startOffset = new Vector3(-1.75f, 0.5f, 0f); 


    public GameObject bookPrefab; 
    public List<Book> myBooks = new List<Book>();

    [ContextMenu("Generate Gradient Books")]
    public void GenerateBooks()
    {
        myBooks.RemoveAll(book => book == null);
        // Safety check to prevent spawning thousands of books if you click it twice
        if (myBooks.Count > 0)
        {
            Debug.LogWarning("Books are already generated on this shelf!");
            return;
        }

        GameObject bookFolder = GameObject.Find(gameObject.name + "_Books");
        if (bookFolder == null)
        {
            bookFolder = new GameObject(gameObject.name + "_Books");
        }

        for (int i = 0; i < booksPerRow; i++)
        {
            
            Vector3 localSlotPos = startOffset + new Vector3(i * spacing, 0, 0);
            Vector3 worldSlotPos = transform.TransformPoint(localSlotPos);

           
            float percentage = (float)i / (booksPerRow - 1); 
            Color exactShade = Color.Lerp(lightColor, darkColor, percentage);

            
            GameObject newBookObj = Instantiate(bookPrefab, worldSlotPos, transform.rotation);
            newBookObj.transform.SetParent(bookFolder.transform);
            newBookObj.name = $"Book_{i}";

            
            Book bookScript = newBookObj.GetComponent<Book>();
            bookScript.InitializeBook(exactShade);

            bookScript.myShelf = this;
         
            myBooks.Add(bookScript);
        }
    }

    public int CalculateBestSlotIndex(Color scannedBookColor)
    {
        int bestSlot = 0;
        float smallestDifference = float.MaxValue;

        for (int i = 0; i < booksPerRow; i++)
        {
            
            float percentage = (float)i / (booksPerRow - 1);
            Color idealSlotColor = Color.Lerp(lightColor, darkColor, percentage);

          
            float difference = Mathf.Abs(scannedBookColor.r - idealSlotColor.r) +
                               Mathf.Abs(scannedBookColor.g - idealSlotColor.g) +
                               Mathf.Abs(scannedBookColor.b - idealSlotColor.b);

           
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestSlot = i;
            }
        }

        return bestSlot;
    }

    public Vector3 GetWorldPositionForSlot(int slotIndex)
    {
        Vector3 localSlotPos = startOffset + new Vector3(slotIndex * spacing, 0, 0);
        return transform.TransformPoint(localSlotPos);
    }

    public Vector3 GetDeliveryZone()
    {
        Vector3 deliveryPosition = transform.position + (transform.forward * 1f);
        float roundedX = Mathf.Round(deliveryPosition.x);
        float roundedZ = Mathf.Round(deliveryPosition.z);
        return new Vector3(roundedX, 0f, roundedZ);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 targetZone = GetDeliveryZone();
        Gizmos.DrawCube(targetZone, new Vector3(0.9f, 0.1f, 0.9f));
        Gizmos.DrawLine(transform.position, targetZone);

        Gizmos.color = Color.cyan;
        for (int i = 0; i < booksPerRow; i++)
        {
            Gizmos.DrawWireCube(GetWorldPositionForSlot(i), new Vector3(0.4f, 0.6f, 0.4f));
        }
    }

    public int FindCorrectSlot(Color targetColor)
    {
        int bestSlot = 0;
        float smallestDifference = float.MaxValue;

       
        Color.RGBToHSV(targetColor, out float targetH, out float targetS, out float targetV);


        for (int i = 0; i < booksPerRow; i++)
        {
            
            float percentage = (float)i / (booksPerRow - 1);
            Color idealSlotColor = Color.Lerp(lightColor, darkColor, percentage);
            
            
            Color.RGBToHSV(idealSlotColor, out float idealH, out float idealS, out float idealV);

            
            float difference = Mathf.Abs(targetH - idealH) + Mathf.Abs(targetS - idealS) + Mathf.Abs(targetV - idealV);

            
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestSlot = i;
            }
        }

        Debug.Log($"Algorithm finished! Closest color match is Slot {bestSlot}");
        return bestSlot;
    }

    public void AcceptBook(Book returnedBook, int exactSlot)
    {
        
        if (!myBooks.Contains(returnedBook))
        {

            myBooks.Add(returnedBook);
        }

        
       int slotIndex = exactSlot;

        
        Vector3 localSlotPos = startOffset + new Vector3(slotIndex * spacing, 0, 0);
        Vector3 worldSlotPos = transform.TransformPoint(localSlotPos);

        
        returnedBook.transform.position = worldSlotPos;
        returnedBook.transform.rotation = transform.rotation;

        
        Rigidbody rb = returnedBook.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        
        GameObject bookFolder = GameObject.Find(gameObject.name + "_Books");
        if (bookFolder != null)
        {
            returnedBook.transform.SetParent(bookFolder.transform);
        }
        else
        {
            returnedBook.transform.SetParent(this.transform);
        }
    }

}
