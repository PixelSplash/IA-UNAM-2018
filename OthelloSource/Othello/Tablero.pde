class Tablero {

    private int[][] tablero;
    private int tamano;
    private int alto;
    private int ancho;

    /**
     * Constructor de Tablero
     */
    Tablero(int x, int y, int tamano) {
        alto = x;
        ancho = y;
        this.tamano = tamano;
        tablero = new int[alto][ancho];
        inicializar();
    }

    void setup() {
        tablero = new int[alto][ancho];
    }

    int getFicha(int x, int y) {
        return tablero[x][y];
    }

    void setFicha(int x, int y, int ficha) {
        tablero[x][y] = ficha;
    }

    /**
     * Metodo que sirve para copiar 2 tableros
     * @return Tablero con la misma configuracion del ya existente
     */
    Tablero copiaTablero() {
        Tablero copia = new Tablero(this.alto, this.ancho, this.tamano);
        for (int i = 0; i < this.alto; i++) {
            for (int j = 0; j < this.ancho; j++) {
                copia.setFicha(i, j, tablero[i][j]);
            }
        }
        return copia;
    }

    /**
     * Metodo que sirve para comparar dos tableros
     *
     * @param copia Tablero
     * @return true en caso de que coordenada a coordenada las fichas sean
     * iguales
     */
    boolean equals(Tablero copia) {
        for (int i = 0; i < alto; i++) {
            for (int j = 0; j < ancho; j++) {
                if (tablero[i][j] != copia.getFicha(i, j)) {
                    return false;
                }
            }
        }
        return true;
    }

    int getTamano() {
        return tamano;
    }

    int getAlto() {
        return alto;
    }

    int getAncho() {
        return ancho;
    }

    /**
     * Metodo ilustrativo que sirve para pintar en consola las configuaciones de
     * los tableros
     *
     * @return
     */
    String pinta() {
        for (int i = 0; i < this.alto; i++) {
            System.out.print("|");
            for (int j = 0; j < this.ancho; j++) {
                System.out.print(" " + tablero[i][j]);
            }
            System.out.println(" |");

        }
        return "";
    }

    /**
     * Metodo ilustrativo que sirve para pintar en consola las configuaciones de
     * los tableros
     *
     * @return
     */
    public String toString() {
        for (int i = 0; i < this.alto; i++) {
            System.out.print("|");
            for (int j = 0; j < this.ancho; j++) {
                System.out.print(" " + tablero[i][j]);
            }
            System.out.println(" |");

        }
        return "";
    }

    /**
     * Metodo que inicializa un tablero a la configuracion inicial de la partida
     */
    void inicializar() {
        tablero[(alto / 2) - 1][(ancho / 2) - 1] = 2;
        tablero[(alto / 2) - 1][ancho / 2] = 1;
        tablero[alto / 2][ancho / 2] = 2;
        tablero[alto / 2][(ancho / 2) - 1] = 1;
    }
     void display() {
    noStroke();
    for (int i = 0; i < tablero.length; i++) {
      for (int j = 0; j < tablero.length; j++) {
        if (tablero[i][j] == 1) {
          fill(0);
          ellipse((j*tamano)+(tamano/2), (i*tamano)+(tamano/2), tamano/2, tamano/2);
          continue;
        }
        if (tablero[i][j]== 2) {
          fill(255);
          ellipse((j*tamano)+(tamano/2), (i*tamano)+(tamano/2), tamano/2, tamano/2);
        }
      }
    }
  }
  int[] numeroFichas(){
   int[] numFichas = new int[2];
   for (int i = 0; i < tablero.length; i++) {
      for (int j = 0; j < tablero.length; j++) {
        if (tablero[i][j] == 1) {
          numFichas[0]++;
        }
        if (tablero[i][j]== 2) {
          numFichas[1]++;
        }
      }
    }
   return numFichas;
  }

}