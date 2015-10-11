public class Prop {
  public int type;
  public int x;
  public int y;
  
  public Prop(int type, int x, int y) {
    this.type = type;
    this.x = x;
    this.y = y;
    props.add(this);
  }
  
  public void display() {
    fill(255,0,0,100); noStroke();
    drawBlock(x, y, ""+type);
  }
  
  public String toString(int offset_x, int offset_y) {
    float z = 0;
    if(type == 0) {z = floor(random(40,80));}
    String str = type + " " + (x-offset_x) + " " + (y-offset_y) + " " + z;
    return str;
  }
}
