/**
 * Representación de un tablero del juego Reversi/Othello.
 * La representación del tablero es mediante un arreglo bidimensional de enteros
 * que representan: 0 = casilla vacia, 1 = disco del jugador 1, 2 = disco del jugador 2.
 * @author  Rodrigo Colín
 * @version %I%, %G%
 */
class Tablero {
  int ancho;
  int alto;
  int tamano;
  int[][] mundo;
  color backgroundColor = color(0, 153, 51);
  color p1Color = color(0, 0, 0);
  color p2Color = color(255, 255, 255);

  /**
   * Constructor por default de un Tablero de 8x8 con tamaño de casilla 100px.
   */
  Tablero() {
    this(8, 8, 100);
  }

  /**
   * Constructor por default de un Tablero de 8x8.
   * @param tamano tamaño en pixeles de cada casilla cuadrada del tablero
   */
  Tablero(int tamano) {
    this(8, 8, tamano);
  }

  /**
   * Constructor principal de un tablero.
   * @param  ancho  anchura, en casillas, del tablero
   * @param  alto   altura, en casillas, del tablero
   * @param  tamano tamaño en pixeles de cada casilla cuadrada del tablero
   */
  Tablero(int ancho, int alto, int tamano) {
    this.ancho = ancho;
    this.alto = alto;
    this.tamano = tamano;
    mundo = new int[alto][ancho];
    for (int i = 0; i < this.alto; i++) {
      for (int j = 0; j < this.ancho; j++) {
        mundo[i][j] = 0;
      }
    }
    // Configuración inicial
    mundo[alto/2][ancho/2] = 1;
    mundo[alto/2-1][ancho/2] = 2;
    mundo[alto/2][ancho/2-1] = 2;
    mundo[alto/2-1][ancho/2-1] = 1;
  }

  /**
   * Constructor de copia para un Tablero.
   * @param mundo representa la configutación que será copiada para crear el nuevo Tablero
   */
  Tablero(int[][] mundo) {
    this(mundo, 100);
  }

  /**
   * Constructor de copia para un Tablero con tamaño de casilla personalizado.
   * @param mundo  representa la configutación que será copiada para crear el nuevo Tablero
   * @param tamano tamaño en pixeles de cada casilla cuadrada del tablero
   */
  Tablero(int[][] mundo, int tamano) {
    ancho = mundo[0].length;
    alto = mundo.length;
    this.tamano = tamano; 
    this.mundo = new int[mundo.length][mundo[0].length];
    for (int i = 0; i < alto; i++) {
      for (int j = 0; j < ancho; j++) {
        this.mundo[i][j] = mundo[i][j];
      }
    }
  }

  /**
   * Conversor de pixel a número de casilla dentro de un Tablero.
   * Útil al dar clic sobre la pantalla y determinar la posición (x ó y) en el Tablero.
   * @param n valor en pixel con respecto a la ventana
   * @return  entero que representa el número de casilla en el Tablero
   */
  int toTileInt(int n) {
    return floor(n/tamano);
  }

  /**
   * Cuenta la cantidad de discos/fichas de un determinado jugador en el Tablero.
   * @param  player identificador del jugador, es decir: 1 = Jugador 1, 2 = Jugador 2
   * @return        cantidad de discos/fichas en el tablero del jugador dado como parámetro 
   */
  int count(int player) {
    int total = 0;
    for (int i = 0; i < alto; i++) {
      for (int j = 0; j < ancho; j++) {
        if (mundo[i][j] == player) {
          total += 1;
        }
      }
    }
    return total;
  }
  
  /**
   * Establece o cambia un disco/ficha del tablero en la posición indicada.
   * Nota: no se realiza ninguna verificación sobre la posición o cambios en los demás discos
   * @param x      posición X en coordenadas del tablero (no en pixeles) donde se estable el disco
   * @param y      posición Y en coordenadas del tablero (no en pixeles) donde se estable el disco
   * @param player identificador del jugador, es decir: 1 = Jugador 1, 2 = Jugador 2
   */
  void setDisk(int x, int y, int player) {
    mundo[y][x] = player;
  }

  /**
   * Determina si la casilla seleccionada es válida para colocar un disco/ficha.
   * TODO: Realizar una validación más exhaustiva, por el momento solo revisa si la casilla es vacia
   * @param x posición X en coordenadas del tablero (no en pixeles) donde se valida la casilla
   * @param y posición Y en coordenadas del tablero (no en pixeles) donde se valida la casilla
   */
  boolean isValid(int x, int y) {
    if (mundo[y][x] == 0)
      return true;
    return false;
  }

  /**
   * Dibuja una representación gráfica del tablero en la ventana de Processing. 
   */
  void display() {
    for (int i = 0; i < alto; i++) {
      for (int j = 0; j < ancho; j++) {
        // Dibujar las casillas del tablero:
        fill(backgroundColor);
        stroke(255);
        rect(j * tamano, i * tamano, tamano, tamano);
        // Dibujar los discos/fichas del tablero:
        if (mundo[i][j] == 1) {
          fill(p1Color);
          noStroke();
          ellipse((j*tamano)+(tamano/2), (i*tamano)+(tamano/2), tamano/2, tamano/2);
        } else if (mundo[i][j] == 2) {
          fill(p2Color);
          noStroke();
          ellipse((j*tamano)+(tamano/2), (i*tamano)+(tamano/2), tamano/2, tamano/2);
        }
      }
    }
  }
}
