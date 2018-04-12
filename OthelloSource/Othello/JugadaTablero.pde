class JugadaTablero {
  private int numFichasDiagonalSI, numFichasDiagonalSD, numFichasDiagonalII, numFichasDiagonalID, numFichasVerticalS, numFichasVerticalI, numFichasIzquierda, numFichasDerecha; 
  JugadaTablero() {    
  }
  /**
   * Metodo que sirve para indicar si una jugada es valida
   */
  boolean jugadaValida(int x, int y , int jugadorActual , Tablero tablero) {
    if(x<0||y<0){
      return false;
    }
    limpiar();
    int jugadorOpuesto = (jugadorActual==1)?2:1;
    int numFichasReversibles = 0; 
    if(tablero.getFicha(x,y) != 0){
     return false ; 
    }
    if ((x-1)>=0) { // REvisamos si existe una fila antes de esta 
      if ((y-1)>=0) { // Izquierda
        numFichasDiagonalSI = diagonalSuperiorIzq(x-1, y-1, jugadorOpuesto,tablero); // Revisada esta bien 
        numFichasReversibles+=numFichasDiagonalSI;
      }
      if ((y+1)<tablero.getAncho()) { // Revisamos derecha 
        numFichasDiagonalSD = diagonalSuperiorDer(x-1, y+1, jugadorOpuesto,tablero);
        numFichasReversibles+=numFichasDiagonalSD; // bien 
      }
      numFichasVerticalS =lineaVerticalSuperior(x-1, y, jugadorOpuesto,tablero); // vertical superior bien
      numFichasReversibles+=numFichasVerticalS;
    }
    if ((x+1)< tablero.getAlto()) { // Revisamos si existe nivel inferior
      if ((y-1)>=0) { // Revisamos si existe una diagonal inferior Izquierda
        numFichasDiagonalII =diagonalInferiorIzq(x+1, y-1, jugadorOpuesto,tablero);
        numFichasReversibles+=numFichasDiagonalII; // bien
      }
      if ((y+1)<tablero.getAncho()) { // Revisamos si existe una diagonal inferior Derecha
        numFichasDiagonalID =diagonalInferiorDer(x+1, y+1, jugadorOpuesto,tablero);
        numFichasReversibles+=numFichasDiagonalID;
      }
      numFichasVerticalI =lineaVerticalInferior(x+1, y, jugadorOpuesto,tablero); // bien 
      numFichasReversibles+=numFichasVerticalI;
    }
    if ((y-1)>=0) {
      numFichasIzquierda =lineaHorizontalIzq(x, y-1, jugadorOpuesto,tablero);
      numFichasReversibles+=numFichasIzquierda;
      //if (numFichasIzquierda>0)
        //println("Izquierda");
    }
    if ((y+1)<tablero.getAncho()) {
      numFichasDerecha =lineaHorizontalDer(x, y+1, jugadorOpuesto,tablero);
      numFichasReversibles+=numFichasDerecha;
    }

    return (numFichasReversibles > 0);
  }
  int diagonalSuperiorIzq(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    boolean jugadaValida = false;
    for (int i= x, j =y; i>=0&&j>=0; i--, j--) {
      if (tablero.getFicha(i, j) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(i, j) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(i, j) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(i, j) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int diagonalSuperiorDer(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    boolean jugadaValida = false;
    for (int i= x, j =y; i>=0 && j<tablero.getAncho(); i--, j++) {
      if (tablero.getFicha(i, j) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(i, j) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(i, j) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(i, j) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int diagonalInferiorIzq(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    boolean jugadaValida = false;
    
    for (int i= x, j =y; i<tablero.getAlto()&&j>=0; i++, j--) {
      
      if (tablero.getFicha(i, j) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(i, j) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(i, j) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(i, j) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int diagonalInferiorDer(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    boolean jugadaValida = false;
    for (int i= x, j =y; i<tablero.getAncho()&&j<tablero.getAlto(); i++, j++) {
      if (tablero.getFicha(i, j) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(i, j) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(i, j) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(i, j) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int lineaVerticalSuperior(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    boolean jugadaValida = false;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    for (int  j =x; j>=0; j--) {
      if (tablero.getFicha(j, y) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(j, y) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(j, y) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(j, y) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int lineaVerticalInferior(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    boolean jugadaValida = false;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    for (int  j =x; j<tablero.getAlto(); j++) {
      if (tablero.getFicha(j, y) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(j, y) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(j, y) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(j, y) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int lineaHorizontalIzq(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    boolean jugadaValida = false;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    for (int  j =y; j>=0; j--) {
      if (tablero.getFicha(x, j) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(x, j) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(x, j) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(x, j) == 0) {
        return  0;
      }
    }
    return  0;
  }
  int lineaHorizontalDer(int x, int y, int jugadorOpuesto,Tablero tablero) {
    int numFichasReversibles = 0 ;
    int jugadorActual = (jugadorOpuesto == 1)?2:1;
    boolean jugadaValida = false;
    for (int  j =y; j<tablero.getAncho(); j++) {
      if (tablero.getFicha(x, j) == jugadorOpuesto) {
        numFichasReversibles++;
        jugadaValida = true;
      }
      if (tablero.getFicha(x, j) == jugadorActual && jugadaValida ) {
        return numFichasReversibles;
      }
      if (tablero.getFicha(x, j) == jugadorActual && !jugadaValida ) {
        return  0;
      }
      if (tablero.getFicha(x, j) == 0) {
        return  0;
      }
    }
    return  0;
  }
  void swap(int x, int y,Tablero tablero, int jugadorActual) {

    if (numFichasDiagonalSI >0) {
      cambiarDiagonalSupIzq(x-1, y-1,tablero,jugadorActual);
    }  
    if (numFichasDiagonalSD>0) {
      cambiarDiagonalSupDer(x-1, y+1,tablero,jugadorActual);
    }

    if (numFichasDiagonalII>0) {
      cambiarDiagonalInfIzq(x+1, y-1,tablero,jugadorActual);
    }
    if (numFichasDiagonalID>0) {
      cambiarDiagonalInfDer(x+1, y+1,tablero,jugadorActual);
    }

    if (numFichasVerticalS>0) {
      cambiarLineaVerticalS(x-1, y,tablero,jugadorActual);
    }

    if (numFichasVerticalI>0) {
      cambiarLineaVerticalI(x+1, y,tablero,jugadorActual);
    }

    if (numFichasIzquierda>0) {
      cambiarLineaHorizontalIzq(x, y-1,tablero,jugadorActual);
    }

    if (numFichasDerecha>0) {
      cambiarLineaHorizontalDer(x, y+1,tablero,jugadorActual);
    }
    limpiar();
    
  }
  void cambiarDiagonalSupIzq(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasDiagonalSI; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      x--;
      y--;
    }
  }
  void cambiarDiagonalSupDer(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasDiagonalSD; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      x--;
      y++;
    }
  }
  void cambiarDiagonalInfIzq(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasDiagonalII; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      x++;
      y--;
    }
  }
  void cambiarDiagonalInfDer(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasDiagonalID; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      x++;
      y++;
    }
  }
  void cambiarLineaVerticalS(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasVerticalS; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      x--;
    }
  }
  void cambiarLineaVerticalI(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasVerticalI; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      x++;
    }
  }
  void cambiarLineaHorizontalIzq(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasIzquierda; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      y--;
    }
  }
  void cambiarLineaHorizontalDer(int x, int y,Tablero tablero, int jugadorActual) {
    for (int i = 0; i<numFichasDerecha; i++) {
      tablero.setFicha(x, y, jugadorActual);  
      y++;
    }
  }
  private void limpiar(){
    numFichasDiagonalSI = 0 ;  
    numFichasDiagonalSD=0; 
    numFichasDiagonalII=0; 
    numFichasDiagonalID=0 ; 
    numFichasVerticalS=0;
    numFichasVerticalI=0; 
    numFichasIzquierda=0; 
    numFichasDerecha=0;
  }
}