public class BinarySearchTreeNode 
{

    public int barcode;   //to better sort books almost like remember slot number  
    public Shelf targetShelf; 
    public int targetSlot;    
    public BinarySearchTreeNode left;
    public BinarySearchTreeNode right;

    public BinarySearchTreeNode(int barcode, Shelf targetShelf, int targetSlot)
    {
        this.barcode = barcode;
        this.targetShelf = targetShelf;
        this.targetSlot = targetSlot;
        left = null;
        right = null;
    }
}
