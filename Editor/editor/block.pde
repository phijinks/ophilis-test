public class Block {
  public int x;
  public int y;
  
  public Block(int x, int y) {
    this.x = x;
    this.y = y;
    blocks.add(this);
  }
  
  public void display() {
    fill(0); noStroke();
    drawBlock(x, y);
  }
}
