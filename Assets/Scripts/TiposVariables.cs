using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiposVariables 
{
    public enum Sexo { Hombre, Mujer };
    public enum Experiencia { Principiante, Intermedio, Avanzado };
    public enum Material { Gimnasio, Casa, Powerlifting };
    public enum CategoriaEjercicio { Basico, BasicoExtra, Accesorio, Complementario }
    public enum TipoEjercicio { Equipado, Deficit, Tempo, Excentrico, Isometrico, Biserie, Superserie };
    public struct Ejercicio
    {
        public float categoria;
        public float series;
        public float repeticiones;
        public float RPE;
    }
    public struct Dia
    {
        public List<Ejercicio> ejercicios;
    }
}
