using UnityEngine;

public class BarcodeDatabase : MonoBehaviour
{
   private BinarySearchTreeNode root;

   
    public void Insert(int id, Shelf shelf, int slot)
    {
        root = InsertRec(root, id, shelf, slot);
    }

    private BinarySearchTreeNode InsertRec(BinarySearchTreeNode root, int id, Shelf shelf, int slot)
    {
        if (root == null)
        {
            root = new BinarySearchTreeNode(id, shelf, slot);
            return root;
        }

       
        if (id < root.barcode)
            root.left = InsertRec(root.left, id, shelf, slot);
        else if (id > root.barcode)
            root.right = InsertRec(root.right, id, shelf, slot);

        return root;
    }

  
    public BinarySearchTreeNode Search(int id)
    {
        return SearchRec(root, id);
    }

    private BinarySearchTreeNode SearchRec(BinarySearchTreeNode root, int id)
    {
     
        if (root == null || root.barcode == id)
            return root;

        
        if (root.barcode > id)
            return SearchRec(root.left, id);

        // Key is smaller than root's key
        return SearchRec(root.right, id);
    }
}
