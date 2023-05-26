using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamenController : MonoBehaviour
{
    [SerializeField] private GameObject[] partes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void valida(string texto)
    {
        // int parte, int respEsperada, int respObtenida
        int parte = int.Parse(texto.Split('|')[0]);
        int respEsperada = int.Parse(texto.Split('|')[1]);
        int respObtenida = int.Parse(texto.Split('|')[2]);
        print("parte:" + parte +" de partes:"+ partes.Length);

        if(respEsperada == respObtenida)
        {
            GameManager.Instance.respuestasExamenCorrectas++;
        }
        else
        {
            GameManager.Instance.respuestasExamenIncorrectas++;
        }
        partes[parte].gameObject.SetActive(false);
        partes[parte+1].gameObject.SetActive(true);
        if (parte+1 == partes.Length-1)
        {
            //GameManager.Instance.UpdateTextCantidades();
            GameManager.Instance.txtCantCorrectas.text = GameManager.Instance.respuestasExamenCorrectas.ToString();
            GameManager.Instance.txtCantInorrectas.text = GameManager.Instance.respuestasExamenIncorrectas.ToString();

             


        }
    }

    public void volver()
    {
        GameManager.Instance.respuestasExamenCorrectas = 0;
        GameManager.Instance.respuestasExamenIncorrectas = 0;
        GameManager.Instance.txtCantCorrectas.text = string.Empty;
        GameManager.Instance.txtCantInorrectas.text = string.Empty;
        partes[0].gameObject.SetActive(true);
        partes[partes.Length- 1].gameObject.SetActive(false);
    }

    public void regresar()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CloseExamen();
    }
}
