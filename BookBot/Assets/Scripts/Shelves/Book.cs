using UnityEngine;

public class Book : MonoBehaviour
{
    public Shelf myShelf;
    public Color myColor; 
    public bool isSorted = true;
    public int barcodeID;

    public float hue {get; private set;}
    public float saturation {get; private set;}
    public float value {get; private set;}


    //used in .....
    public void InitializeBook(Color newColor)
    {
        myColor = newColor;
        Renderer renderer = GetComponent<Renderer>();

        if(renderer != null)
        {
            renderer.material.color = myColor;
        }
        else
        {
        Debug.LogWarning("this book as no renderer");    
        }
        

        //for better performance, doing math here
        //HSV better for color search
        Color.RGBToHSV(myColor, out float H, out float S, out float V);

        hue = H;
        saturation = S;
        value = V;
    }
}

