using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNeuronalEntrenador : MonoBehaviour
{
    [SerializeField]
    private ValoresNeuronas valoresNeuronas;

    [SerializeField]
    public int[] neuronasPorCapa = { 3, 3, 3 };

    //Entreno
    public float[,] entreno;

    //Red Neuronal
    private RedNeuronalMulticapa red;

    //Sigleton de la Red Neuronal del entreno
    public static RedNeuronalEntrenador instance;

    public int printModulo = 10;
    
    //Carga el entrenamiento del Scriptable Object
    public void CargarEntrenamiento()
    {
        entreno = new float[valoresNeuronas.entrenamiento.Length, 6];
      
        for (int i = 0; i < valoresNeuronas.entrenamiento.Length; ++i)
        {
            entreno[i, 0] = valoresNeuronas.entrenamiento[i].intensidadDeseada;
            entreno[i, 1] = valoresNeuronas.entrenamiento[i].intensidadObtenida;
            entreno[i, 2] = valoresNeuronas.entrenamiento[i].fatiga;
            entreno[i, 3] = valoresNeuronas.entrenamiento[i].incrementarCarga;
            entreno[i, 4] = valoresNeuronas.entrenamiento[i].decrementarCarga;
            entreno[i, 5] = valoresNeuronas.entrenamiento[i].descarga;
        }                                                    
    }                                                        
                                                             
    // Start is called before the first frame update         
    void Start()
    {
        instance = this;
        red = new RedNeuronalMulticapa(neuronasPorCapa);
        print(red.PrintRed());
        CargarEntrenamiento();
        EntrenarRed();
        print(red.PrintRed());
    }

    void EntrenarRed()
    {
        float error = 1;
        int epoch = 0;
        string log = "";

        while ((error > 0.05f) && (epoch < 50000))
        {
            error = 0;

            for (int i = 0; i < entreno.GetLength(0); i++)
            {
                for (int j = 0; j < neuronasPorCapa[0]; j++)
                {
                    red.SetInput(j, entreno[i, j]);
                }

                for (int j = neuronasPorCapa[0]; j < entreno.GetLength(1); j++)
                {
                    red.SetOutputDeseado(j - neuronasPorCapa[0], entreno[i, j]);
                }

                red.FeedForward();
                error += red.CalcularError();
                red.BackPropagation();
            }

            error /= entreno.GetLength(0);

            if (epoch % printModulo == 0)
            {
                log += "EPOCH: " + epoch + " ERROR: " + error + "\n";
            }

            epoch++;
        }
        log += "EPOCH: " + epoch + " ERROR: " + error + "\n";
        print(log);
    }
    
    public float[] ConsultarAccion(float[] input)
    {
        for (int i = 0; i < neuronasPorCapa[0]; i++)
        {
            red.SetInput(i, input[i]);
        }
        red.FeedForward();
        return red.GetOutput();
    }
    
}
