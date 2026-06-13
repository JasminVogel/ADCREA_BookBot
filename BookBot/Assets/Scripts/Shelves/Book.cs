using UnityEngine;

public class Book : MonoBehaviour
{
    public Shelf myShelf;
    public Color myColor; 
    public bool isSorted = true;
    public int barcodeID;

    public float Hue {get; private set;}
    public float Saturation {get; private set;}
    public float Value {get; private set;}


    //used in .....
    public void InitializeBook(Color newColor)
    {
        myColor = newColor;
        Renderer renderer = GetComponent<Renderer>();

        if(renderer != null)
        {
            renderer.material.color = myColor;
        }

        //for better performance, doing math here
        Color.RGBToHSV(myColor, out float H, out float S, out float V);

        Hue = H;
        Saturation = S;
        Value = V;
    }
}

