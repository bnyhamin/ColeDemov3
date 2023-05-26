using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class PizarraVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;
    private RectTransform _rectransform;
    void Awake()
    {
        _rectransform = GetComponent<RectTransform>();

    }
    // Start is called before the first frame update
    void Start()
    {
        LoadVideo();
        _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        _rectransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

        _rectransform.anchorMin = new Vector2(0, 0);
        _rectransform.anchorMax = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadVideo()
    {
        int idcontenido = PlayerPrefs.GetInt("varglobal_idContenido");
        string temp = Application.dataPath + "/Sprites/Clases/Contenido/"+idcontenido+"/video.mp4";
        print("temp_contenido_videoclases:"+ temp);
        //if (video.url == temp) return;
        video.url = temp;
        video.Prepare();
        video.Play();
        video.loopPointReached += CheckOver;
        
    }

    void CheckOver(VideoPlayer vp)
    {
        Debug.Log("Acumular puntos");
        CerrarVideo();
        //GameManager.Instance.CloseVideoClases();
    }

    public void CerrarVideo()
    {
        DestroyImmediate(gameObject);
    }
}
