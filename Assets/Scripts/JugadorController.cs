using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugadorController : MonoBehaviour
{
    public static JugadorController instance;
    // Start is called before the first frame update

    private Rigidbody rb;

    //Inicializo el contador de coleccionables recolectados
    private int contador;

    //Para poder modificar la velocidad
    public float velocidad;

    //Inicializo las variaables que contienen los textos de la interfaz
    public Text textContador, textGanar,textNivel;

    public float gameDuration = 120f; // Duración del juego en segundos
    private float timer; // Contador de tiempo

    public GameObject completionSign;
    //Inicializo la variable de nivel
    public int nivel;
    private Scene currentScene;

    private void Awake() {
        if(instance==null){
            instance = this; 
            DontDestroyOnLoad(gameObject);       
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Capturo la variable al inicial el juego
        rb=GetComponent<Rigidbody>();

        //Inicializo el contador
        contador = 0;

        //Actualizo el texto del contador
        setTextContador();

        //Inicio del tex
        textGanar.text="";

        timer = gameDuration;

        //Inicializo el nivel
        nivel = 1;

        currentScene = SceneManager.GetActiveScene();
    }
    void FixedUpdate(){
        //Estas variables nos capturan el movimiento horizontal y vertical del teclado
        float movimientoH=Input.GetAxis("Horizontal");
        float movimientoV=Input.GetAxis("Vertical");

        //Un vector 3 es un trío de posiciones en el espacio XYZ, en este caso el que corresponde al movimiento
 
        Vector3 movimiento=new Vector3(movimientoH,0.0f,movimientoV);

        //Asigno el movimiento a Rigibody
        rb.AddForce(movimiento*velocidad);

        // Actualizar el temporizador cada frame
        timer -= Time.deltaTime;

        // Verificar si el tiempo ha alcanzado cero
        // if(currentScene.name == "Juego"){
        //      if (timer < 0f)
        //      {
        //          LoseGame(); // Llama a la función de perder el juego
        //     }
        // }
        
    }

    //Se ejecuta al entrar a un objeto con la opción isTrigger seleccionada
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Coleccionable"))
        {
            other.gameObject.SetActive(false);

            //Aumento el contador en uno
            contador ++;

            //Actualizo el texto del contador
            setTextContador();
        }
    }
        public void CompleteGame()
    {
        completionSign.SetActive(true);
        Invoke("ReturnToHome", 5f);
    }

    void ReturnToHome()
    {
        SceneManager.LoadScene("Home");
    }

    //Actualizo el texto del contador
    void setTextContador(){
        textContador.text = "Contador: " + contador.ToString();
        textNivel.text = "Nivel: " + nivel.ToString();

        if (contador >= 12)
        {
            if(timer >= 0){
                 Debug.Log("¡Ganaste!");
                 textGanar.text = "¡Ganaste!";
                 Invoke("CompleteGame", 2f);

                 nivel++;

                
                if(currentScene.name == "Juego"){
                 textGanar.text = "¡Ganaste!";
                 Invoke("CompleteGame", 2f);
                 nivel++;
                 timer=0;
                 SceneManager.LoadScene("Juego-01");
                }
                else if(currentScene.name == "Juego-01"){
                 textGanar.text = "¡Ganaste!";
                 Invoke("CompleteGame", 2f);
                 nivel++;
                 timer=0;
                 SceneManager.LoadScene("Juego-02");
                 }
                else if(currentScene.name == "Juego-02"){
                    textGanar.text = "¡Ganaste!";
                 Invoke("CompleteGame", 2f);
                 nivel++;
                 timer=0;
                 SceneManager.LoadScene("Juego-03");
                 }

            }
            else{
                LoseGame();
            }

        }
    }

    void LoseGame()
    {
        Debug.Log("Tiempo agotado. ¡Has perdido!");
        SceneManager.LoadScene("Home"); // Carga la escena home
    }

}
