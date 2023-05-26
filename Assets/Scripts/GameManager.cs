using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int time = 30;
    public int difficulty = 1;
    public string[] cursos = null;
    public AudioClip[] audios;
    [SerializeField] int score;
    public GameObject pausePanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject prefabPanelDialog;
    [SerializeField] private GameObject panelStatus;
    [SerializeField] private GameObject VideoClase;
    [SerializeField] private GameObject[] ejercicios;
    [SerializeField] private GameObject[] examenes; 
    [SerializeField] private GameObject _canvasCurso;
    [SerializeField] private GameObject panelContenido;

    public GameObject panelEjercicios;
    public GameObject panelExamenes;
    public Text felicitaciones;
    public GameObject personajePanel;
    public GameObject[] personajes;
    [SerializeField] private GameObject _camCanvas;

    [SerializeField] private int idUsuario;
    [SerializeField] private string nombreUsuario;
    [SerializeField] private string correo;
    [SerializeField] private string dni;
    [SerializeField] private int puntaje;
    [SerializeField] private int idPersonaje;
    [SerializeField] private int grado;
    [SerializeField] private string nivel;
    [SerializeField] private bool _puntero_visible=true;
    [SerializeField] GameObject prefabButtonContenido;
    public int respuestasExamenCorrectas=0;
    public int respuestasExamenIncorrectas=0;
    public Text txtCantCorrectas;
    public Text txtCantInorrectas;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            //UIManager.Instance.UpdateUIScore(score);

        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.UpdateUIScore(score);
        //Time.timeScale = 1;
        
        idUsuario = PlayerPrefs.GetInt("varglobal_idUsuario");
        nombreUsuario = PlayerPrefs.GetString("varglobal_nombreUsuario");
        puntaje = PlayerPrefs.GetInt("varglobal_puntaje");
        correo = PlayerPrefs.GetString("varglobal_correo");
        idPersonaje = PlayerPrefs.GetInt("varglobal_idPersonaje");
        grado = PlayerPrefs.GetInt("varglobal_grado");
        nivel = PlayerPrefs.GetString("varglobal_nivel");

        //si no tiene personaje configurado abre panel para escoger personaje
        print("idUsuario:" + idUsuario);
        print("idPersonaje:" + idPersonaje);
        UIManager.Instance.UpdateUINombre(nombreUsuario);

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "SceneGame") return;
        if (idPersonaje == 0)
        {
            personajePanel.SetActive(true);
            _camCanvas.SetActive(true);
            panelStatus.SetActive(false);
        }
        else
        {
            choisePersonaje();
            _camCanvas.SetActive(false);
            panelStatus.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            //SceneManager.LoadScene("Menu");
            //pausePanel.SetActive(true);
            
            PunteroVisible(_puntero_visible);

        }
    }

    public void PunteroVisible(bool visible)
    {
        print("visible:"+visible);
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            _camCanvas.SetActive(true);
            panelStatus.SetActive(true);
            _puntero_visible = false; //al gatillar cambia a este valor para la siguiente vez
            cleanPersonaje();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            _camCanvas.SetActive(false);
            panelStatus.SetActive(true);
            _puntero_visible = true;//al gatillar cambia a este valor para la siguiente vez
            choisePersonaje();
        }


    }

    public void PunteroVisiblePizarra()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        panelStatus.SetActive(false);
        _puntero_visible = true;
    }

    public void startTime()
    {
        //GameObject.Find("Time").SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        yield return new WaitForSeconds(1);
        /*while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            UIManager.Instance.UpdateUITime(time);
        }
        if (time <= 0)
        {
            UIManager.Instance.showUITime(false);
        }*/
    }

    public void Unpause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Ajustes()
    {
        Debug.Log("Ajustes");
        personajePanel.SetActive(true);
        //_camCanvas.SetActive(true);
    }


    public void ChangePersonaje(int id_personaje)
    {
        StartCoroutine(Change_Personaje(id_personaje));
    }

    IEnumerator Change_Personaje(int id_personaje)
    {
        idPersonaje = id_personaje;
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/actualizar_personaje.php?id_usuario=" + idUsuario + "&id_personaje=" + idPersonaje);
        yield return (conneccion);
        if (conneccion.text == "201")
        {
            personajePanel.SetActive(false);
            choisePersonaje();
            print("registró correctamente");
        }
        else
        {
            Debug.LogError("Error en la conección con la base de datos");
        }
    }

    public void cleanPersonaje()
    {
        for(int i = 0; i < personajes.Length; i++)
        {
            personajes[i].SetActive(false);
        }
        //GameObject.Find("Man").SetActive(false);
        //GameObject.Find("Woman").SetActive(false);
    }

    
    private void choisePersonaje()
    {        
        cleanPersonaje();
        if (!_puntero_visible)
        {   
            Time.timeScale = 1;
            _camCanvas.SetActive(false);
            panelStatus.SetActive(true);
            _puntero_visible = true;
        }
            _camCanvas.SetActive(false);
        if (idPersonaje == 1) personajes[0].SetActive(true);
        if (idPersonaje == 2) personajes[1].SetActive(true);
        
    }

    public void showLivingInformation(bool fgvisible, string text)
    {        
        string mensaje = string.Empty;
        if(text == "ColliderAula1")
        {
            mensaje = "Bienvenido al Aula de Historia!." +
                "                          " +
                "En este curso conocerás la Historia del Perú y del Mundo";
        }
        prefabPanelDialog.SetActive(fgvisible);
        UIManager.Instance.ChangeDialog(mensaje);
        StartCoroutine(WaitingInClass(2));
    }

    IEnumerator WaitingInClass(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("SceneSalon");
    }


    public void Logout()
    {
        Debug.Log("Logout");
        Time.timeScale = 1;
        SceneManager.LoadScene("SceneLogin");
    }

    public void OpenVideoClases()
    {
        //Debug.Log("Activar Videoclases");
        //panelVideoClase.SetActive(true);
        panelStatus.SetActive(false);
        GameObject video = Instantiate(VideoClase);
        //video.transform = assetRoot.transform.position;
        //Instantiate(VideoClase, new Vector3(0, 0, 0), Quaternion.identity);
        video.transform.parent = canvas.transform;
    }


    public void OpenEjercicios()
    {
        panelEjercicios.SetActive(true);
        int id_contenido = PlayerPrefs.GetInt("varglobal_idContenido");
        //string _path = Application.dataPath + "/Sprites/Clases/Contenido/" + idcontenido + "/ejercicio.prefab";
        //GameObject ejercicio = (GameObject)AssetDatabase.LoadMainAssetAtPath(_path);
        //GameObject video = Instantiate(VideoClase);        
        //video.transform.parent = canvas.transform;
        if(id_contenido == 6)
        {
            ejercicios[0].gameObject.SetActive(true);
        }
        if (id_contenido == 10)
        {
            ejercicios[1].gameObject.SetActive(true);
        }

        //GameObject objectButton = Instantiate(prefabButtonContenido);
        //objectButton.transform.parent = panelContenido.transform;



    }

    public void OpenExamen()
    {
        panelExamenes.SetActive(true);
        int id_contenido = PlayerPrefs.GetInt("varglobal_idContenido");
        //string _path = Application.dataPath + "/Sprites/Clases/Contenido/" + idcontenido + "/ejercicio.prefab";
        //GameObject ejercicio = (GameObject)AssetDatabase.LoadMainAssetAtPath(_path);
        //GameObject video = Instantiate(VideoClase);        
        //video.transform.parent = canvas.transform;
        if (id_contenido == 7)
        {
            examenes[0].gameObject.SetActive(true);
        }

        if (id_contenido == 11)
        {
            examenes[1].gameObject.SetActive(true);
        }

        //GameObject objectButton = Instantiate(prefabButtonContenido);
        //objectButton.transform.parent = panelContenido.transform;



    }

    public void CloseExamen()
    {
        panelExamenes.SetActive(false);
    }

    /*public void CloseVideoClases()
    {
        //Debug.Log("Activar Videoclases");
        //panelVideoClase.SetActive(false);
        _canvasCurso.SetActive(false);
        panelStatus.SetActive(true);

    }*/


    public void abrirCurso(int id_curso)
    {
        //panelVideoClase.SetActive(false);
        _canvasCurso.SetActive(true);
        print("ENTRO AL CURSO" + id_curso);
        PlayerPrefs.SetInt("varglobal_idCurso", id_curso);
    }

    public void abrirLienzo()
    {
        panelContenido.SetActive(true);
        //llamar a corrutina para que despliegue
        StartCoroutine(contenidos(PlayerPrefs.GetInt("varglobal_idCurso"))); 
    }

    public void seleccionContenido(int idcontenido)
    {
        print("Asignando a la memoria contenido:" + idcontenido);
        PlayerPrefs.SetInt("varglobal_idContenido", idcontenido);
        panelContenido.SetActive(false);

    }


    IEnumerator contenidos(int id_curso)
    {

        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/contenidos.php?id_curso=" + id_curso);
        yield return (conneccion);
        print("DEBUG conneccion.text:"+conneccion.text);
        if (conneccion.text == "401")
        {
            print("ID Curso incorrecto");
            //textError.text = "Usuario incorrecto";
            //MostrarError("Usuario incorrecto");
        }
        else
        {

            print("substring:" + conneccion.text);
            string[] nDatos = conneccion.text.Substring(0, conneccion.text.Length - 1).Split("-");
            GameManager.Instance.cursos = nDatos;
            RectTransform _rectransform;
            int j = 0;
            for (float i = 0; i < nDatos.Length; i++)
            {

                GameObject objectButton = Instantiate(prefabButtonContenido);
                objectButton.transform.parent = panelContenido.transform;
                _rectransform = objectButton.GetComponent<RectTransform>();
                _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
                _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
                _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
                _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

                _rectransform.anchorMin = new Vector2(0.02f, (0.9f - (i / 10)));
                _rectransform.anchorMax = new Vector2(1.0f, (1.0f -  (i / 10)));
                objectButton.transform.GetChild(0).GetComponent<Text>().text = nDatos[j].ToString().Split("|")[2];
                //objectButton.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.abrirCurso(nDatos[j].ToString().Split("|")[0]));
                int id_contenido = int.Parse(nDatos[j].Split("|")[0]);
                print("ID_CONTENIDO:" + id_contenido);
                objectButton.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<GameManager>().seleccionContenido(id_contenido));
                


                j++;
            }



        }

    }

    public void UpdateTextCantidades()
    {
        print("Resp correctas:"+ respuestasExamenCorrectas.ToString());
        print("Resp INcorrectas:" + respuestasExamenIncorrectas.ToString());
        txtCantCorrectas.text = respuestasExamenCorrectas.ToString();
        txtCantInorrectas.text = respuestasExamenIncorrectas.ToString();
    }




}
