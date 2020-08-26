using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capa
{
    public int numeroNeuronas;         
    public float[,] pesos;              // Conexión con capa siguiente.
    public float[] biasValores;
    public float[] biasPesos;
    public float[] valoresNeuronas;     // Cálculo del feed forward.

    public float[,] pesosIncremento;    // Para aprendizaje de back propagation, modificación 
                                        // de los pesos en cada ciclo de aprendizaje.
    public float[] valoresDeseados;     // Para aprendizaje, valor que debería haber tenido.
    public float[] errores;             // Para aprendizaje, diferencia entre lo deseado y lo obtenido.

    public Capa capaAnterior, capaSiguiente;

    public Capa(int nNeuronas)
    {
        numeroNeuronas = nNeuronas;
    }
  
    public void Inicializar(Capa anterior, Capa siguiente)
    {
        valoresNeuronas = new float[numeroNeuronas];
        valoresDeseados = new float[numeroNeuronas];
        errores = new float[numeroNeuronas];

        capaAnterior = anterior;

        if(siguiente != null) // Capa  de entrada o hidden.
        {
            capaSiguiente = siguiente;
            pesos = new float[numeroNeuronas, siguiente.numeroNeuronas];
            pesosIncremento = new float[numeroNeuronas, siguiente.numeroNeuronas];

            biasValores = new float[siguiente.numeroNeuronas];
            biasPesos = new float[siguiente.numeroNeuronas];

            for (int i = 0; i < biasValores.Length; i++)
            {
                biasValores[i] = -1;
            }
        }
        else // Capa de salida.
        {
            capaSiguiente = null;
            pesos = null;
            pesosIncremento = null;
            biasValores = null;
            biasPesos = null;
        }
    }

    public void AsignarPesosAleatorios()
    {
        if(capaSiguiente != null)
        {
            for (int i = 0; i < numeroNeuronas; i++)
            {
                for (int j = 0; j < capaSiguiente.numeroNeuronas; j++)
                {
                    pesos[i, j] = Random.Range(-1f, 1f);
                }
            }
            for (int i = 0; i < capaSiguiente.numeroNeuronas; i++)
            {
                biasPesos[i] = Random.Range(-1f, 1f);
            }
        }
    }

    public void CalcularValoresNeuronas()
    {
        if(capaAnterior != null)
        {
            for (int j = 0; j < numeroNeuronas; j++)
            {
                float x = 0;
                for (int i = 0; i < capaAnterior.numeroNeuronas; i++)
                {
                    x += capaAnterior.valoresNeuronas[i] * capaAnterior.pesos[i, j];
                }
                x += capaAnterior.biasValores[j] * capaAnterior.biasPesos[j];

                if(capaSiguiente == null && Const.OUTPUT_LINEAL)
                {
                    valoresNeuronas[j] = x;
                }
                else
                {
                    valoresNeuronas[j] = 1.0f / (1 + Mathf.Exp(-x));
                }
            }
        }
    }

    public void CalcularErrores()
    {
        // Capa Output
        if(capaSiguiente == null)
        {
            for (int i = 0; i < numeroNeuronas; i++)
            {
                errores[i] = (valoresDeseados[i] - valoresNeuronas[i]) * 
                               valoresNeuronas[i] * 
                               (1 - valoresNeuronas[i]);
            }
        }
        // Capa Hidden
        else if (capaAnterior != null)
        {
            for (int i = 0; i < numeroNeuronas; i++)
            {
                float suma = 0;
                for (int j = 0; j < capaSiguiente.numeroNeuronas; j++)
                {
                    suma += capaSiguiente.errores[j] * pesos[i, j];
                }
                errores[i] = suma * valoresNeuronas[i] * (1 - valoresNeuronas[i]);
            }
        }
    }

    public void AjustarPesos()
    {
        if (capaSiguiente != null)
        {
            for (int i = 0; i < numeroNeuronas; i++)
            {
                for (int j = 0; j < capaSiguiente.numeroNeuronas; j++)
                {
                    //float dw = Const.RATIO_APRENDIZAJE * capaSiguiente.errores[j] * capaSiguiente.valoresNeuronas[j];
                    float dw = Const.RATIO_APRENDIZAJE * capaSiguiente.errores[j] * valoresNeuronas[j];
                    if (Const.USO_INERCIA)
                    {
                        pesos[i, j] += dw + Const.RATIO_INERCIA * pesosIncremento[i, j];
                        pesosIncremento[i, j] = dw;
                    }
                    else
                    {
                        pesos[i, j] += dw;
                    }
                }
            }
            for (int j = 0; j < capaSiguiente.numeroNeuronas; j++)
            {
                float dw = Const.RATIO_APRENDIZAJE * capaSiguiente.errores[j] * biasValores[j];
                biasPesos[j] += dw;
            }
        }
    }

    public string PrintCapa()
    {
        string res = "";
        if (capaSiguiente != null)
        {
            res += "PESOS\n";
            for (int i = 0; i < numeroNeuronas; i++)
            {
                res += "\ni" + i + " ->";
                for (int j = 0; j < capaSiguiente.numeroNeuronas; j++)
                {
                    res += "nj" + j + ": " + pesos[i, j].ToString("0.00") + " ";
                }
            }
            for (int j = 0; j < capaSiguiente.numeroNeuronas; j++)
            {
                res += "\nbj" + j + " " + biasPesos[j];
            }
        }
        else
        {
            res += "OUTPUT\n";
            for (int i = 0; i < numeroNeuronas; i++)
            {
                res += "ni" + i + " " + valoresNeuronas[i].ToString("0.00") + "/" + valoresDeseados[i].ToString("0.00");
            }
        }
        return res;
    }
}
