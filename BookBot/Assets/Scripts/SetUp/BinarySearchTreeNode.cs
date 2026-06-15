public class BinarySearchTreeNode 
{

    public int barcode;     
    public Shelf targetShelf; 
    public int targetSlot;    
    public BinarySearchTreeNode left;
    public BinarySearchTreeNode right;

    public BinarySearchTreeNode(int id, Shelf shelf, int slot)
    {
        barcode = id;
        targetShelf = shelf;
        targetSlot = slot;
        left = null;
        right = null;
    }
}
