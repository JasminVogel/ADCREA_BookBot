using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public Shelf[] allShelves;
    public Transform dropZone;


    public float bookThickness = 0.2f;

    public Stack<Book> pileOfBooks = new Stack<Book>();
  

    public void StartSimulation()
    {
        BuildDatabase();
        GenerateChaos();
    }

    private void BuildDatabase()
    {
        BarcodeDatabase db = GetComponent<BarcodeDatabase>();
        
        // Loop through every shelf in the warehouse
        foreach (Shelf shelf in allShelves)
        {
            // Loop through all the perfectly sorted books before the chaos starts
            for (int i = 0; i < shelf.myBooks.Count; i++)
            {
                Book book = shelf.myBooks[i];
                
                // 1. Generate a random 5-digit UPC barcode
                int randomUPC = Random.Range(10000, 99999);
                book.barcodeID = randomUPC;
                
                // 2. Insert the barcode and its physical location into the BST!
                db.Insert(randomUPC, shelf, i);
            }
        }
        
        Debug.Log("Boss: Successfully built the Binary Search Tree Database!");
    }
    public void GenerateChaos()
    {
        pileOfBooks.Clear();
        List<Book> allAvailableBooks = new List<Book>();
        foreach (Shelf shelf in allShelves)
        {
            allAvailableBooks.AddRange(shelf.myBooks);
        }

        if (allAvailableBooks.Count == 0)
        {
            Debug.LogWarning("No books found!");
            return;
        }

        int scatterCount = Mathf.Min(SimulationSettings.booksToGenerate, allAvailableBooks.Count);
        
        for (int i = 0; i < scatterCount; i++)
        {
            
            int randomIndex = Random.Range(0, allAvailableBooks.Count);
            Book chosenBook = allAvailableBooks[randomIndex];

         
            allAvailableBooks.RemoveAt(randomIndex);
            if (chosenBook == null)
            {
                i--; 
                continue;
            }    

            Shelf parentShelf = chosenBook.myShelf;
            if (parentShelf != null)
            {
                parentShelf.myBooks.Remove(chosenBook);
            }

           
            chosenBook.transform.SetParent(null); 
            
            //for a better feel 
            /*
            Rigidbody rb = chosenBook.GetComponent<Rigidbody>();
            if (rb == null) rb = chosenBook.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false; 

            rb.mass = 3f;
            rb.linearDamping = 1f;
            rb.angularDamping = 2f;
            */
            BoxCollider col = chosenBook.GetComponent<BoxCollider>();
            if (col == null) col = chosenBook.gameObject.AddComponent<BoxCollider>();
           
           
            Vector3 messyNudge = new Vector3(
                Random.Range(-0.05f, 0.05f), 
                (i * bookThickness)+0.6f, // Stack them upwards, starting 1 unit above the dropzone
                Random.Range(-0.05f, 0.05f)
            );
            
           
            chosenBook.transform.position = dropZone.position + messyNudge;
            
            float randomTwist = Random.Range(-20f, 20f);
            chosenBook.transform.rotation = Quaternion.Euler(0f, randomTwist, 90f);
            chosenBook.isSorted = false;
            pileOfBooks.Push(chosenBook);
        }

        Debug.Log($"Chaos generated! Scattered {scatterCount} books into the pile.");
        RobotController robot = GameObject.FindFirstObjectByType<RobotController>();
        if (robot != null)
        {
            Debug.Log("Boss: Wake up Robot, there is a new mess to clean!");
            robot.SwitchState(new IdleState(robot));
        }
    }

}


