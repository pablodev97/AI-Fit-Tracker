using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValoresNeuronas", menuName = "Scriptable/ValoresNeuronas", order = 2)]
public class ValoresNeuronas : ScriptableObject, ISerializationCallbackReceiver
{
    public int amount = 7;
    private int oldAmount = 10;

    public bool saved = true;

    public EntrenamientoRed[] entrenamiento;

    public void OnAfterDeserialize()
    {
        if (!saved && oldAmount != amount)
        {
            oldAmount = amount;
            EntrenamientoRed[] temp = new EntrenamientoRed[amount];

            for (int i = 0; i < entrenamiento.Length && i < temp.Length; ++i)
            {
                temp[i].intensidadDeseada = entrenamiento[i].intensidadDeseada;
                temp[i].intensidadObtenida = entrenamiento[i].intensidadObtenida;
                temp[i].fatiga = entrenamiento[i].fatiga;
                temp[i].incrementarCarga = entrenamiento[i].incrementarCarga;
                temp[i].decrementarCarga = entrenamiento[i].decrementarCarga;
                temp[i].descarga = entrenamiento[i].descarga;
            }
            entrenamiento = new EntrenamientoRed[amount];
            entrenamiento = temp;
        }
    }

    public void OnBeforeSerialize() { }
}

[System.Serializable]
public struct EntrenamientoRed
{
    // Input
    public float intensidadDeseada;
    public float intensidadObtenida;
    public float fatiga;

    // Outputs
    public float incrementarCarga;
    public float decrementarCarga;
    public float descarga;
}
