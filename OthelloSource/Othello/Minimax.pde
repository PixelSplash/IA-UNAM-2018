import java.util.ArrayList;

class Minimax {
  private final int debug = 0;
  private ConfiguracionTablero configuracion;
  private int jugadorActual; 
  private int dificultad;
  private int profundidadArbol;
  private ArrayList<Node<Tablero>> hojas;
  Minimax(int jugadorActual, int dif) {
    this.jugadorActual = jugadorActual;
    hojas = new ArrayList<Node<Tablero>>();
    configuracion = new ConfiguracionTablero();
    dificultad = dif;
    if(dificultad == 1)profundidadArbol = 1;
    if(dificultad == 2)profundidadArbol = 2;
    if(dificultad == 3)profundidadArbol = 5;
  }
  /**
   * Metodo que regresa las coordenadas en las que se deberan tirar la computadora siguiedo el algoritmo de minimax 
   * @param tablero tablero actual del juego 
   * @return  arreglo de integers que representan la posicion que en la que se tiro la ficha
   */
  public int[] elecionMinimax(Tablero tablero) {
    Tablero original = tablero.copiaTablero(); // por si las dudas
    Node<Tablero> arbol = new Node<Tablero>(tablero);

    //ConfiguracionTablero desiciones = new ConfiguracionTablero();
    arbol = configuracion.crearConfiguracionesTablero(jugadorActual, arbol, profundidadArbol);
    int valor = minimax(arbol, profundidadArbol, true);
    println("El valor de la Heuristica es = "+ valor);
    
    Node<Tablero> mejorJugada = null;
    for (Node<Tablero> hoja : hojas) {
      if (hoja.getUtilidad() == valor) {
        mejorJugada = hoja;
        break;
      }
    }
    return jugada(mejorJugada, original);
  }
  private int[] jugada(Node<Tablero> arbol, Tablero inicial) {
    int[] posiciones = new int[2];
      if (inicial.equals(arbol.getParent().getData())) {
        posiciones[0] = 0 ;
        posiciones[1] = 0 ;
        Tablero actual = arbol.getData();
        for (int i=0; i < inicial.getAlto(); i++) {
          for (int j = 0; j<inicial.getAncho(); j++) {
            if (inicial.getFicha(i, j) ==0 && jugadorActual == actual.getFicha(i, j)) {
              posiciones[0] = i ;
              posiciones[1] = j ;
              return posiciones;
            }
          }
        }
        return posiciones;
      } else {
        return jugada(arbol.getParent(), inicial);
      }
      
    
  }
  private int minimax(Node<Tablero> nodo, int depth, boolean maximizingPlayer) {
    
    int bestValue;
    if (depth == 0 || nodo.isLeaf()) {
      //if(debug == 1)print("finale\n");
      int valor = configuracion.heuristica(maximizingPlayer, nodo.getData());
      //if(debug == 1)print(valor);
      //if(debug == 1)print("\n");
      nodo.setUtilidad(valor);
      hojas.add(nodo);
      return valor;
    }
    if (maximizingPlayer) {
      if(debug == 1)print("maxi\n");
      bestValue = Integer.MIN_VALUE;
      for (Node<Tablero> child : nodo.getChildren()) {
        int v = minimax(child, depth-1, false);
        bestValue = Math.max(bestValue, v);
      }
    } else {
      if(debug == 1)print("mini\n");
      bestValue = Integer.MAX_VALUE;
      for (Node<Tablero> child : nodo.getChildren()) {
        int v = minimax(child, depth-1, true);
        bestValue = Math.min(bestValue, v);
      }
    }
    if(debug == 1)print(nodo.getData());
    if(debug == 1)print(bestValue+ "\n");
    return bestValue;
  }
}

class ConfiguracionTablero {
  private JugadaTablero jugadaValida ;
  public ConfiguracionTablero () {
    jugadaValida = new JugadaTablero();
  } 
  /**
   * Metodo que sirve como fachada para mandarlo a llamar y que regrese el nodo raiz 
   * @param jugadorActual jugador que busca ampliar su busqueda
   * @param configuracion  Nodo inicial que contiene la configuracion del tablero en la que se quiere buscar la solucion
   * @param depth profundidad que se desea explorar 
   * @return    */
   Node<Tablero> crearConfiguracionesTablero(int jugadorActual, Node<Tablero> configuracion, int depth){
     crearConfiguraciones(jugadorActual, configuracion,depth);
     Node<Tablero> padre = configuracion;
      while (!padre.isRoot()) {
        //println(padre.getData());
        padre = padre.getParent();
        
      }
      return padre;
  }
   /**
    * Metodo recursivo que sirve  para crear las configuraciones validas de un jugador
    * @param jugadorActual  jugador que buscara maximizar sus posibilidades de ganar 
    * @param configuracion nodo del arbol de las  configuraciones sobre el cual se esta recursando
    * @param depth profundidad que se desea construir el arbol de configuraciones
    */
  void crearConfiguraciones(int jugadorActual, Node<Tablero> configuracion, int depth) {
    
    if (depth ==  0) {
      return ;
    } else {
      
      Tablero tab = configuracion.getData();
      
      for (int i = 0; i<tab.getAlto(); i++) {
        
        for (int j = 0; j<tab.getAlto(); j++) {
         
          if (jugadaValida.jugadaValida(i, j, jugadorActual, tab)) {
              Tablero hijo = tab.copiaTablero();
              hijo.setFicha(i, j, jugadorActual);
            jugadaValida.swap(i, j, hijo, jugadorActual);
            configuracion.addChild(hijo);
          } else {
            if (i == tab.getAlto()-1 && j == tab.getAncho()-1 && configuracion.isLeaf()) {
              Tablero hijo = tab.copiaTablero();
              configuracion.addChild(hijo);
            }
          }
        }
      }
      int jugadorActual2 = (jugadorActual == 1)?2: 1;
      for (Node<Tablero> a : configuracion.getChildren()) {
        crearConfiguraciones(jugadorActual2, a, depth-1);
      }
    }
  }
  /**
   * Metodo que regresa  el valor de una configuracion de unn  tablero para el jugador que desea saberlo
   * @param jugador 
   * @param tablero configuracion de un tablero de othelo 
   * @return 
   */
  int heuristica(boolean jug, Tablero tablero) {
    int dif = 3;
    int score=0;
    int jugador = (!jug)?2:1;
    int oponente = (jugador == 1)?2:1;
    
    for (int i =0; i<8; i++) {
      for (int j=0; j<8; j++) {
        if (tablero.getFicha(i, j) == 2) {
          if(dif > 0){        
            score++;
          }
          if(dif > 1){
            if(i == 0 || j == 0 || i == 7 || j == 7){
              score+= 5;
              //print("Si entro");
              //if(jugador == 2)score+= 20;
            }
          }
          if(dif > 2){
            if((i == 0 && (j == 7 || j == 0)) || (i == 7 && (j == 7 || j == 0))){
              score+= 5;
              //print("Si entro");
              //if(jugador == 2)score+= 50;
            }
          }
        }
        else if (tablero.getFicha(i, j) == 1){
          if(dif > 0){        
            score--;
          }
          if(dif > 1){
            if(i == 0 || j == 0 || i == 7 || j == 7){
              score-= 10;
            }
            
            
          }
          if(dificultad > 2){
            if((i == 0 && (j == 7 || j == 0)) || (i == 7 && (j == 7 || j == 0))){
              score-= 10;
            }
          }
        }
        
      }
    }
    
   print(tablero);
   print(score);
   print("\n");
    return score;
  }
}