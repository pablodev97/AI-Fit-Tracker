using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNeuronal 
{
    private Capa capaEntrada, capaOculta, capaSalida;

    public RedNeuronal(int nNeuronasInput, int nNeuronasHidden, int nNeuronasOutput)
    {
        capaEntrada = new Capa(nNeuronasInput);
        capaOculta = new Capa(nNeuronasHidden);
        capaSalida = new Capa(nNeuronasOutput);

        capaEntrada.Inicializar(null, capaOculta);
        capaOculta.Inicializar(capaEntrada, capaSalida);
        capaSalida.Inicializar(capaOculta, null);

        capaEntrada.AsignarPesosAleatorios();
        capaOculta.AsignarPesosAleatorios();
    }

    public void SetInput(int i, float valor)
    {
        capaEntrada.valoresNeuronas[i] = valor;
    }
    public float GetOutput(int i)
    {
        return capaSalida.valoresNeuronas[i];
    }

    public void FeedForward()
    {
        //capaEntrada.CalcularValoresNeuronas();
        capaOculta.CalcularValoresNeuronas();
        capaSalida.CalcularValoresNeuronas();
    }
    public int GetMaxOutputId()
    {
        int id = -1;
        float max = float.MinValue;
        for (int i = 0; i < capaSalida.numeroNeuronas; i++)
        {
            if(capaSalida.valoresNeuronas[i] > max)
            {
                max = capaSalida.valoresNeuronas[i];
                id = i;
            }
        }
        return id;
    }

    public void SetOutputDeseado(int i, float valor)
    {
        capaSalida.valoresDeseados[i] = valor;
    }
    public void BackPropagation()
    {
        capaSalida.CalcularErrores();
        capaOculta.CalcularErrores();

        capaOculta.AjustarPesos();
        capaEntrada.AjustarPesos();
    }

    public float CalcularError()
    {
        float error = 0;
        for (int i = 0; i < capaSalida.numeroNeuronas; i++)
        {
            error += Mathf.Pow(capaSalida.valoresNeuronas[i] - capaSalida.valoresDeseados[i], 2);
        }
        error /= capaSalida.numeroNeuronas;
        return error;
    }

    public string PrintRed()
    {
        string res = "";
        res += "CAPA ENTRADA\n" + capaEntrada.PrintCapa() + "\n\n";
        res += "CAPA OCULTA\n" + capaOculta.PrintCapa() + "\n\n";
        res += "CAPA SALIDA\n" + capaSalida.PrintCapa() + "\n";
        return res;
    }
}
