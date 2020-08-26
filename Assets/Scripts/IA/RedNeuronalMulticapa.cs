using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNeuronalMulticapa 
{
    Capa[] capas;

    public RedNeuronalMulticapa(int[] nNeuronas)
    {
        capas = new Capa[nNeuronas.Length];
        for (int i = 0; i < nNeuronas.Length; i++)
        {
            capas[i] = new Capa(nNeuronas[i]);
        }
        for (int i = 0; i < nNeuronas.Length; i++)
        {
            if (i == 0) capas[i].Inicializar(null, capas[i + 1]);
            else if (i == nNeuronas.Length - 1) capas[i].Inicializar(capas[i - 1], null);
            else capas[i].Inicializar(capas[i - 1], capas[i + 1]);

            if (i < nNeuronas.Length - 1) capas[i].AsignarPesosAleatorios();
        }
    }

    public void SetInput(int i, float valor)
    {
        capas[0].valoresNeuronas[i] = valor;
    }

    public float[] GetOutput()
    {
        return capas[capas.Length-1].valoresNeuronas;
    }

    public void FeedForward()
    {
        for (int i = 1; i < capas.Length; i++)
        {
            capas[i].CalcularValoresNeuronas();
        }
    }

    public int GetMaxOutputId()
    {
        int id = -1;
        float max = float.MinValue;
        for (int i = 0; i < capas[capas.Length - 1].numeroNeuronas; i++)
        {
            if(capas[capas.Length - 1].valoresNeuronas[i] > max)
            {
                max = capas[capas.Length - 1].valoresNeuronas[i];
                id = i;
            }
        }
        return id;
    }

    public void SetOutputDeseado(int i, float valor)
    {
        capas[capas.Length - 1].valoresDeseados[i] = valor;
    }

    public void BackPropagation()
    {

        for (int i = capas.Length -1; i > 0; i--)
        {
            capas[i].CalcularErrores();
        }
        for (int i = capas.Length - 2; i >= 0; i--)
        {
            capas[i].AjustarPesos();
        }
    }

    public float CalcularError()
    {
        float error = 0;
        for (int i = 0; i < capas[capas.Length - 1].numeroNeuronas; i++)
        {
            error += Mathf.Pow(capas[capas.Length - 1].valoresNeuronas[i] - capas[capas.Length - 1].valoresDeseados[i], 2);
        }
        error /= capas[capas.Length - 1].numeroNeuronas;
        return error;
    }

    public string PrintRed()
    {
        string res = "";

        for (int i = 0; i < capas.Length; i++)
        {
            res += "CAPA "+i+"\n" + capas[i].PrintCapa() + "\n\n";
        }
        return res;
    }
}
