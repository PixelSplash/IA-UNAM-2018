/*Representación de un movimiento.
*Este tipo de dato se guarda en el arbol para mantener
*la información de las tiradas en todo momento.
*/
public class Configuracion{
  public int valor;
  public int x;
  public int y;
  public int[][] mundo;
  
  public Configuracion(int valor, int x, int y, int[][] mundo){
    this.valor = valor;
    this.x = x;
    this.y = y;
    this.mundo = mundo;
  }
  
  public Configuracion(int x, int y, int[][] mundo){
    this.x = x;
    this.y = y;
    this.mundo = mundo;
  }
  
  public Configuracion(int valor){
    this.valor = valor;
  }
  
  public Configuracion(int[][] mundo){
    this.mundo = mundo;
  }
  
/**
* Cuenta la cantidad de discos/fichas de un determinado jugador en el Tablero.
* @param  player identificador del jugador, es decir: 1 = Jugador 1, 2 = Jugador 2
* @return        cantidad de discos/fichas en el tablero del jugador dado como parámetro 
*/
  public int count(int player) {
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
}
