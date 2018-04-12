int jugadorActual = 1;
int alto = 8;
int ancho = 8;
int tamano = 100 ;
int dificultad = 1;
Tablero tablero;
ConfiguracionTablero configuracion;
JugadaTablero jugada;

void keyPressed() {
  if (key == '1'){
    print("Dificultad cambiada a 1\n");
    dificultad = 1;
  }
  if (key == '2'){
    print("Dificultad cambiada a 2\n");
    dificultad = 2;
  }
  if (key == '3'){
    print("Dificultad cambiada a 3\n");
    dificultad = 3;
  }
}

void settings() {
  size(tamano*8, tamano*8);
}
void setup() {
  tablero = new Tablero(alto, ancho, tamano);
  configuracion = new ConfiguracionTablero();
  jugada = new JugadaTablero();
  background(0, 153, 51);
  stroke(181, 242, 192);
  for (int i = 0; i<alto; i++) {
    line(tamano*i, 0, tamano*i, tamano*8);
    line(0, tamano*i, tamano*8, tamano*i);
  }
}

void turno() {

  if (jugada.jugadaValida((mouseY/tamano), (mouseX/tamano), jugadorActual, tablero)) {
    tablero.setFicha((mouseY/tamano), (mouseX/tamano), jugadorActual);
    jugada.swap((mouseY/tamano), (mouseX/tamano), tablero, jugadorActual);
    tablero.display();
    if (tieneJugadaValida(oponente())) {
      jugadorActual = 2;
    } else {
      jugadorActual = 1;
    }
  }
}

void draw() {
  try {
    Thread.sleep(100);
  }
  catch(Exception e) {
  }
  if (!tieneJugadaValida(jugadorActual)&&!tieneJugadaValida(oponente())) {
    finJuego();
  } else {
    
    tablero.display();
    if (jugadorActual == 2) {
      agente() ;
    }
  }
}
void mouseClicked() {
  turno();
}
/**
 * @return int que represente al oponente del jugador actual
 */
int oponente() {
  return (jugadorActual==1)?2:1;
}

/**
 * Metodo que decide si un jugador tiene una jugada valida
 * @param int jugador 
 * @return true si existe una jugada valida en otro caso false   
 */
boolean tieneJugadaValida(int jugador) {
  for (int i = 0; i<tablero.getAlto(); i++) {
    for (int j = 0; j<tablero.getAlto(); j++) {
      if (jugada.jugadaValida(i, j, jugador, tablero)) {
        return true;
      }
    }
  }
  return false;
}
void agente() {
  Minimax tiro = new Minimax(2,dificultad);
  int[] posicionTiro = tiro.elecionMinimax(tablero);
  println("Posicion  de tiro de Minimax = (" +posicionTiro[0]+","+posicionTiro[1]+")." );
  if (jugada.jugadaValida(posicionTiro[0], posicionTiro[1], 2, tablero)) {
    tablero.setFicha(posicionTiro[0], posicionTiro[1], 2);
    jugada.swap(posicionTiro[0], posicionTiro[1], tablero, 2);
  }     
  if (tieneJugadaValida(oponente())) {
    jugadorActual = 1;
  } else {
    jugadorActual = 2;
  }
}
void finJuego() {
  background(255);
  stroke(175);
  textFont(createFont("Arial", 16, true));       
  fill(0);
  textAlign(CENTER);
  text("Juego Terminado. Negras =" +tablero.numeroFichas()[0]+"  Blancas = "+ tablero.numeroFichas()[1]+" .", width/2, 60);
  if (tablero.numeroFichas()[0] < tablero.numeroFichas()[1]) {
    text("Ganan Blancas.", width/2, 120);
  }
  if (tablero.numeroFichas()[0] > tablero.numeroFichas()[1]) {
    text("Ganan Negras.", width/2, 120);
  }
  if (tablero.numeroFichas()[0] == tablero.numeroFichas()[1]) {
    text("Empate.", width/2, 120);
  }
}