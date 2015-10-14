public class Rectangle {
  public int x1;
  public int y1;
  public int x2;
  public int y2;
  boolean fill;
  
  public Rectangle(int x, int y, boolean fill) {
    this.x1 = x;
    this.y1 = y;
    this.fill = fill;
  }
  
  public void display(int x, int y) {
    fill(200, 255, 200, 200); noStroke();
    int a1; int b1;
    int a2; int b2;
    if(x1 > x) {a1 = x; a2 = x1;} else {a1 = x1; a2 = x;}
    if(y1 > y) {b1 = y; b2 = y1;} else {b1 = y1; b2 = y;}
    for(int a=a1; a<=a2; a++) {
      for(int b=b1; b<=b2; b++) {
        drawBlock(a, b);
      }
    }
  }
  
  public void finish(int x, int y) {
    this.x2 = x;
    this.y2 = y;
    
    if(x1 > x2) {
      int temp = x2;
      x2 = x1;
      x1 = temp;
    }
    if(y1 > y2) {
      int temp = y2;
      y2 = y1;
      y1 = temp;
    }
    
    for(int a = x1; a <= x2; a++) {
      for(int b = y1; b <= y2; b++) {
        if(!fill) {
          boolean isBlock = false;
          for(Block block : blocks) {
            if(block.x == a && block.y == b) {isBlock = true; break;}
          }
          if(!isBlock) {new Block(a, b);}
        } else {
          removeBlock(a, b);
        }
      }
    }
  }
}
