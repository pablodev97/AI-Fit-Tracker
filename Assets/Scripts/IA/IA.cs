using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class IA : MonoBehaviour
{
    //private RedNeuronal red;
    private RedNeuronalMulticapa red;
    public int[] neuronasPorCapa = { 4, 3, 3 };
    public int neuronasInput = 4, neuronasHidden = 3, neuronasOutput = 3;
    public int printModulo = 10;

    public static IA instance;

    //public RegistroEntrenamiento[] setEntrenamiento;

    // INPUT: Nº Aliados, %Vida, PJ Luchando, Distancia a PJ
    // OUPUT: Perseguir, Huir, Movimiento en grupo
    private static float[,] setEntrenamiento =
    {
        {0, 1, 0, 0.2f,      0.9f, 0.1f, 0.1f},
        {0, 1, 1, 0.2f,      0.9f, 0.1f, 0.1f},
        {0.8f, 1, 0, 0.5f,   0.1f, 0.9f, 0.1f},
        {1, 0.2f, 1, 0.2f,   0.9f, 0.1f, 0.1f},
        {0, 0.2f, 1, 0.2f,   0.1f, 0.1f, 0.9f},
        {1, 0.1f, 0, 0.8f,   0.1f, 0.1f, 0.9f},
    };
    // AÑADIR MÁS COMBINACIONES. POR COMBINATORIA UNAS 54 COMBINACIONES GENERALES.
    void Start()
    {
        instance = this;
        red = new RedNeuronalMulticapa(neuronasPorCapa);
        print(red.PrintRed());
        EntrenarRed();
        print(red.PrintRed());

        //setEntrenamiento = new RegistroEntrenamiento[15];
        //for (int i = 0; i < setEntrenamiento.Length; i++)
        //{
        //    setEntrenamiento[i].registro = new float[7];
        //}
    }
    void EntrenarRed()
    {
        float error = 1;
        int epoch = 0;
        string log = "";
        while((error > 0.05f) && (epoch < 50000))
        {
            error = 0;
            for (int i = 0; i < setEntrenamiento.GetLength(0); i++)
            {
                for (int j = 0; j < neuronasInput; j++)
                {
                    red.SetInput(j, setEntrenamiento[i, j]);
                }
                for(int j = neuronasInput; j < setEntrenamiento.GetLength(1); j++)
                {
                    red.SetOutputDeseado(j - neuronasInput, setEntrenamiento[i, j]);
                }
                red.FeedForward();
                error += red.CalcularError();
                red.BackPropagation();
            }
            error /= setEntrenamiento.GetLength(0);
            if(epoch%printModulo == 0)
            {
                log += "EPOCH: " + epoch + " ERROR: " + error + "\n";
            }
            epoch++;
        }
        log += "EPOCH: " + epoch + " ERROR: " + error + "\n";
        print(log);
    }

    public void ReentrenarRed(float[] input, float[] output)
    {
        float error = 1;
        int epoch = 0;
        string log = "";
        while ((error > 0.1f) && (epoch < 1000))
        {
            for (int j = 0; j < neuronasInput; j++)
            {
                red.SetInput(j, input[j]);
            }
            for (int j = 0; j < neuronasOutput; j++)
            {
                red.SetOutputDeseado(j, output[j]);
            }
            red.FeedForward();
            error = red.CalcularError();
            red.BackPropagation();

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
        for (int i = 0; i < neuronasInput; i++)
        {
            red.SetInput(i, input[i]);
        }
        red.FeedForward();
        return red.GetOutput();
    }

}
