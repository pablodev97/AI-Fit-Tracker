using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Usuario : MonoBehaviour
{
    #region UI
    public Text txtintensidadDeseada;
    public Text txtintensidadObtenida;
    public Text txtfatiga;
    public Text txtOutput1;
    public Text txtOutput2;
    public Text txtOutput3;
    #endregion

    // ----------------- INPUT USUARIO -----------------
    //
    [Header("Input Usuario")]
    [SerializeField]
    [Range(0, 1)]
    public float intensidadDeseada;
    [SerializeField]
    [Range(0, 1)]
    public float intensidadObtenida;
    [SerializeField]
    [Range(0, 1)]
    public float fatiga;
    [SerializeField]

    private float[] input;

    // ------------------- OUTPUT -------------------
    //

    float[] output;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CrearEntrenamiento", 0.5f);
    }

    void SetInput()
    {
        input = new float[3];
        
        input[0] = intensidadDeseada;
        input[1] = intensidadObtenida;
        input[2] = fatiga;
    }

    void CrearEntrenamiento()
    {
        SetInput();

        output = RedNeuronalEntrenador.instance.ConsultarAccion(input);

        SetUI();
    }

    void SetUI()
    {
        txtintensidadDeseada.text = "Intensidad deseada: " + intensidadDeseada;
        txtintensidadObtenida.text = "Intensidad obtenidad: " + intensidadObtenida;
        txtfatiga.text = "Fatiga: " + fatiga;

        txtOutput1.text = "Incremento volumen: " + output[0];
        txtOutput2.text = "Decremento volumen: " + output[1];
        txtOutput3.text = "Descarga: " + output[2];
    }
}
