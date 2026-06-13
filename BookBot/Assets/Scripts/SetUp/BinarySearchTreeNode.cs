public class BinarySearchTreeNode 
{

    public int barcode;       // The Key
    public Shelf targetShelf; // The Value
    public int targetSlot;    // The Value
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
