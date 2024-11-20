using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuRegistro : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField passwordVerify;
    [SerializeField] private TextMeshProUGUI indicacion;

    [SerializeField] Button botonRegistro;
    [SerializeField] Button botonAtras;

    private string errorMessage;

    public bool estado;
    public bool passCoinciden;

    //objeto de la clase firebasemanager para registrar al usuario en la base de datos
    private FirebaseManager fbm;

    private void Start()
    {
        fbm = new FirebaseManager();
        botonRegistro.onClick.AddListener(() => botonRegistrar());
        botonAtras.onClick.AddListener(() => volverAtras());
    }

    private void Update()
    {
        //aquí mostramos el mensaje en el textField. No se puede hacer desde las comprobaciones de firebase porque es un hilo secundario y no deja modificar el textField
        if (!string.IsNullOrEmpty(errorMessage))
        {
            indicacion.text = errorMessage;
            Invoke("limpiarCampo", 5f);
            errorMessage = ""; // Limpiar el mensaje de error después de mostrarlo
        }
    }

    //MIRAR VIDEO DE AUTH FIREBASE PARA ESTE CODIGO EN CASO DE QUERERLO

    //para comprobar que los campos estan rellenos
    private void comprobarCampos()
    {
        if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(passwordField.text) || string.IsNullOrEmpty(passwordVerify.text))
        {
            estado = false;
            passCoinciden = false;
        }
        //esto seria para cuando añada campo de verificacion
        else if (passwordField.text != passwordVerify.text)
        {
            estado = true;
            passCoinciden = false;
        }
        else 
        {
            estado = true;
            passCoinciden = true;
        }
    }

    private void botonRegistrar()
    {
        comprobarCampos();
        if (!estado)
        {
            indicacion.text = "No puede haber un campo vacío";
            Invoke("limpiarCampo", 2f);
        }
        else if (!passCoinciden) 
        {
            indicacion.text = "Las contraseñas no coinciden";
            Invoke("limpiarCampo", 2f);
        }
        else
        {
            var auth = FirebaseAuth.DefaultInstance;
            auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    errorMessage = "Ha ocurrido un error al registrar la cuenta: " + task.Exception.GetBaseException().Message;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    errorMessage = "Ha ocurrido un error al registrar la cuenta: " + task.Exception.GetBaseException().Message;
                    return;
                }

                // Firebase user has been created.
                AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                //ahora registramos este usuario en la base de datos
                fbm.insertarNuevoUsuario(result.User.UserId, registrado =>
                {
                    if (registrado)
                    {
                        indicacion.text = "Cuenta creada correctamente";
                        Invoke("limpiarCampo", 2f);
                        Invoke("cambiarEscena", 2f);
                    }
                });
            });



        }
    }

    private void limpiarCampo()
    {
        indicacion.text = string.Empty;
    }

    private void volverAtras()
    {
        SceneManager.LoadScene("MenuPreInicio");
    }

    private void cambiarEscena() 
    {
        SceneManager.LoadScene("MenuInicio");
    }

}
