using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public Shelf[] allShelves;
    public Transform dropZone;
    public RobotController robot;


    public float bookThickness = 0.2f;

    public Stack<Book> pileOfBooks = new Stack<Book>();
  

    public void StartSimulation()
    {
        BuildDatabase();
        GenerateChaos();
    }

    private void BuildDatabase()
    {
        BarcodeDatabase database = GetComponent<BarcodeDatabase>();
        
        
        foreach (Shelf shelf in allShelves)
        {
            
            for (int i = 0; i < shelf.myBooks.Count; i++)
            {
                Book book = shelf.myBooks[i];
                
                
                int randomUPC = Random.Range(10000, 99999);
                book.barcodeID = randomUPC;
                
              
                database.Insert(randomUPC, shelf, i);
            }
        }
        
        Debug.Log("Built Binary Search Tree Database!");
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

            //select books
            allAvailableBooks.RemoveAt(randomIndex);
            if (chosenBook == null)
            {
                i--; 
                continue;
            }    


            //remove book   
            Shelf parentShelf = chosenBook.myShelf;
            if (parentShelf != null)
            {
                parentShelf.myBooks.Remove(chosenBook);
            }

           
            chosenBook.transform.SetParent(null); 
                       
            Vector3 messyNudge = new Vector3(
                Random.Range(-0.05f, 0.05f), 
                (i * bookThickness)+0.6f, // books stacking
                Random.Range(-0.05f, 0.05f)
            );
            
           
            chosenBook.transform.position = dropZone.position + messyNudge;
            //make it fancy
            float randomTwist = Random.Range(-20f, 20f);
            chosenBook.transform.rotation = Quaternion.Euler(0f, randomTwist, 90f);
            chosenBook.isSorted = false;
            pileOfBooks.Push(chosenBook);
        }

        Debug.Log("books are now in pile");
        if (robot != null)
        {
            Debug.Log("pile of books is ready to be sorted");
            robot.SwitchState(new IdleState(robot));
        }
    }

}


