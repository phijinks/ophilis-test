import javax.swing.JOptionPane;

float vx = 0;
float vy = 0;

int tile = 25;

boolean[] camera_move = new boolean[4];

ArrayList<Block> blocks = new ArrayList<Block>();
ArrayList<Prop> props = new ArrayList<Prop>();
Rectangle r;
boolean menu = false;

PFont font;

int start_x = 0;
int start_y = 0;

void setup() {
  size(800, 600);
  ellipseMode(RADIUS);
  font = loadFont("avenir.vlw");
  textFont(font);
  textAlign(CENTER, CENTER);
}

void draw() {
  background(200, 240, 180);
  
  float spd = 8;
  if(camera_move[0]) {vx += spd;}
  if(camera_move[1]) {vy += spd;}
  if(camera_move[2]) {vx -= spd;}
  if(camera_move[3]) {vy -= spd;}
  
  pushMatrix();
  
  translate((int)(0.5*width-vx), (int)(0.5*height-vy));
  for(Block b : blocks) {
    b.display();
  }
  for(Prop p : props) {
    p.display();
  }
  int[] c = getCoords();
  
  if(r != null) {r.display(c[0], c[1]);}
  
  noFill(); stroke(255, 200); strokeWeight(2);
  drawBlock(c[0], c[1]);
  
  fill(255, 160); noStroke();
  ellipse(tile * ((float)start_x + 0.5), tile * ((float)start_y + 0.5), tile*0.4, tile*0.4);
  
  popMatrix();
  
  if(menu) {
    pushStyle();
    fill(80, 120, 80);
    rectMode(CENTER);
    rect(width/2, height/2, width - 100, height - 100);
    fill(255);
    text("menu", width/2, 100);
    textAlign(LEFT, CENTER);
    textFont(font, 18);
    text("n - new level\ne - export level\nl - load level", 100, 200);
    popStyle();
  }
}

int[] getCoords() {
  int x = floor((mouseX - width/2 + vx) / (float)tile);
  int y = floor((mouseY - height/2 + vy) / (float)tile);
  int[] coords = {x, y};
  return coords;
}

void drawBlock(int x, int y) {drawBlock(x, y, "");}
void drawBlock(int x, int y, String str) {
  rect(tile*x, tile*y, tile, tile);
  pushStyle();
  fill(255);
  text(str, tile*((float)x+0.5), tile*((float)y+0.5));
  popStyle();
}

void keyPressed() {
  if(menu) {
    if(key == 'e') {exportLevel();}
    if(key == 'l') {loadLevel();}
  } else {
    if(key == '\\') {
      menu = true;
      for(int i=0; i<4; i++) {camera_move[i] = false;}
    }
    int dir = "dsaw".indexOf(key);
    if(dir != -1) {camera_move[dir] = true;}
    
    if(key == ' ') {
      if(blocks.size() > 0) {
        float avgX = 0;
        float avgY = 0;
        for(Block b : blocks) {
          avgX += (float)(b.x+0.5) * tile;
          avgY += (float)(b.y+0.5) * tile;
        }
        avgX /= blocks.size();
        avgY /= blocks.size();
        vx = avgX; vy = avgY;
      } else {
        vx = 0;
        vy = 0;
      }
    }
    
    if(keyCode == DELETE || keyCode == BACKSPACE) {
      int[] c = getCoords();
      ArrayList<Prop> garbage = new ArrayList<Prop>();
      for(Prop p : props) {
        if(p.x == c[0] && p.y == c[1]) {
          garbage.add(p);
        }
      }
      for(Prop p : garbage) {props.remove(p);}
    }
    
    try {
      int propnum = Integer.parseInt(String.valueOf(key));
      int[] c = getCoords();
      new Prop(propnum, c[0], c[1]);
    } catch(Exception e) {
      // do nothing lol
    }
  }
}

void keyReleased() {
  if(menu) {
    if(key == '\\') {menu = false;}
  } else {
    int dir = "dsaw".indexOf(key);
    if(dir != -1) {camera_move[dir] = false;}
  }
}

void mousePressed() {
  if(r != null) {return;} // don't do anything if a rectangle's already being drawn
  int[] c = getCoords();
  r = new Rectangle(c[0], c[1], mouseButton == LEFT);
}

void mouseReleased() {
  if(r == null) {return;}
  int[] c = getCoords();
  r.finish(c[0], c[1]);
  r = null;
}

void removeBlock(int x, int y) {
  ArrayList<Block> garbage = new ArrayList<Block>();
  for(Block b : blocks) {
    if(b.x == x && b.y == y) {
      garbage.add(b);
    }
  }
  for(Block b : garbage) {blocks.remove(b);}
}

void exportLevel() {
  String name = JOptionPane.showInputDialog("Level Name:");
  exportLevel(name);
  menu = false;
}
void loadLevel() {
  String name = JOptionPane.showInputDialog("Level Name:");
  loadLevel(name);
  menu = false;
}

void exportLevel(String name) {
  if(blocks.size() == 0) {return;}
  
  int offset_x = blocks.get(0).x;
  int offset_y = blocks.get(0).y;
  int w = offset_x;
  int h = offset_y;
  for(Block b : blocks) {
    if(b.x < offset_x) {offset_x = b.x;}
    if(b.y < offset_y) {offset_y = b.y;}
    if(b.x > w) {w = b.x;}
    if(b.y > h) {h = b.y;}
  }
  w -= offset_x-1;
  h -= offset_y-1;
  
  int padding = 6;
  w += padding*2;
  h += padding*2;
  offset_x -= padding;
  offset_y -= padding;
  
  int[][] data = new int[w][h];
  for(int x=0; x<w; x++) {
    for(int y=0; y<h; y++) {
      data[x][y] = 3;
    }
  }
  
  for(Block b : blocks) {
    int x = b.x - offset_x;
    int y = b.y - offset_y;
    //println(x, y);
    data[x][y] = 0;
  }
  
  for(int x=0; x<w; x++) {
    for(int y=0; y<h; y++) {
      if(data[x][y] == 0) {continue;}
      int maxDist = 6;
      int dist = 1;
      while(dist <= maxDist && y-dist >= 0 && data[x][y-dist] != 0) {
        dist++;
      }
      if((dist <= 1) || (y-dist >= 0 && random(1) > ((float)dist / (float)maxDist))) {
        data[x][y] = (random(1) < 0.5) ? 1 : 2;
      }
    }
  }
  
  String[] levelfile = new String[h+1];
  levelfile[0] = "# " + h;
  for(int y=0; y<h; y++) {
    String line = "";
    for(int x=0; x<w; x++) {
      if(x == start_x-offset_x && y == start_y-offset_y) {
        line = line + "$";
        continue;
      }
      line = line + data[x][y];
    }
    levelfile[y+1] = line;
  }
  saveStrings(name + "_blocks.txt", levelfile);
  
  String[] propsfile = new String[props.size()];
  for(int i=0; i<propsfile.length; i++) {
    propsfile[i] = props.get(i).toString(offset_x, offset_y);
  }
  saveStrings(name + "_props.txt", propsfile);
}

void loadLevel(String name) {
  String[] blocksfile = loadStrings(name + "_blocks.txt");
  String[] propsfile = loadStrings(name + "_props.txt");
  
  blocks.clear();
  props.clear();
  
  for(int y=1; y<blocksfile.length; y++) {
    String line = blocksfile[y];
    for(int x=0; x<line.length(); x++) {
      String b = line.substring(x, x+1);
      if(b.equals("0") || b.equals("$")) {
        new Block(x, y-1);
        if(b.equals("$")) {
          start_x = x;
          start_y = y-1;
        }
      }
    }
  }
  
  for(int i=0; i<propsfile.length; i++) {
    String[] data = propsfile[i].split(" ");
    int type = Integer.parseInt(data[0]);
    int x = Integer.parseInt(data[1]);
    int y = Integer.parseInt(data[2]);
    
    new Prop(type, x, y);
  }
}
