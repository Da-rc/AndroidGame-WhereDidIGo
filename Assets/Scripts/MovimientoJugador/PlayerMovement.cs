using UnityEngine;
using UnityEngine.UI;

//Script para controlar el movimiento del jugador
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private BoxCollider2D coll; //colision
    private SpriteRenderer sprite;
    private Animator anim;//para coger las animaciones y manejar sus condiciones

    private LayerMask jumpableGround; //para la capa de suelo

    private bool isJump, isLeft, isRight;

    private float speedForce, speedForceAir, jumpForce;

    [SerializeField] private AudioSource jumpSoundEffect;

    //Para controlar las animaciones y el cambio entre ellas
    private enum MovementState { idle, walking, jumping }
    private MovementState state;


    // Start is called before the first frame update
    private void Start()
    {
        player=GetComponent<Rigidbody2D>();
        coll=GetComponent<BoxCollider2D>();
        sprite=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();

        jumpableGround = LayerMask.GetMask("Ground");

        player.drag = 2f; //controla el LineaDrag evitando que el personaje se deslize mucho cuanod se suelta el botón de movimiento
        player.gravityScale = 2f; //al modificar el linearDrag tmb afecta a la gravedad, por eso es necesario cambiarla también

        isJump=false;//controla si se ha pulsado el botón o no
        isLeft = false;//controlar botón ir izq
        isRight = false;//controlar boton ir drch

        speedForce = 900;
        speedForceAir = 700;
        jumpForce = 16f;
    }

    // Update is called once per frame
    private void Update()
    {
        movimiento();

        //SOLO PARA TECLADO EN PC
        movimientoPC();

    }

    //----------------------------------------
    //METODOS PARA DETECTAR EL PULSADO DE LOS BOTONES DE MOVIMIENTO
    //----------------------------------------
    public void pushJump()
    {
        isJump = true;
    }
    public void pushLeft()
    {
        isLeft = true;
    }
    public void releaseLeft()
    {
        isLeft = false;
    }
    public void pushRight()
    {
        isRight = true;
    }
    public void releaseRight()
    {
        isRight = false;
    }

    //-----------------------------------------------------------------------------------------------
    //METODOS QUE CONTROLAN QUÉ MOVIMIENTOS HACE EL PERSONAJE EN CADA FRAME(SE LE LLAMA DESDE EL UPDATE)
    //-----------------------------------------------------------------------------------------------
    private void movimiento() 
    {
        movHorizontal();
        movSalto();
        anim.SetInteger("State", (int)state);//pasa a la animación el numero de la que corresponde según la situación
    }

    //Controla el movimiento horizontal, si es hacia izq o drch y modifica el sprite del personaje según la situación
    private void movHorizontal() {
        if (isLeft)
        {
            state = MovementState.walking;//selecciona la animación para este estado(andando)
            sprite.flipX = true;//cambia la dirrección del sprite hacia el lado izquierdo

            if (isGrounded())//Si está en el suelo la velocidad de movimiento será diferente a si está en el aire
            {
                player.AddForce(new Vector2(-speedForce, 0) * Time.deltaTime);
            }
            else
            {
                player.AddForce(new Vector2(-speedForceAir, 0) * Time.deltaTime);
            }

        }
        else if (isRight)
        {
            state = MovementState.walking;//selecciona la animación para este estado(andando)
            sprite.flipX = false;//cambia la dirrección del sprite hacia el lado derecho

            if (isGrounded())//Si está en el suelo la velocidad de movimiento será diferente a si está en el aire
            {
                player.AddForce(new Vector2(speedForce, 0) * Time.deltaTime);
            }
            else
            {
                player.AddForce(new Vector2(speedForceAir, 0) * Time.deltaTime);
            }

        }
        else//En caso de que no esté andando hacia un lado u otro, deberá mostrarse la animación idle
        {
            state= MovementState.idle;
        }
    }

    //controla el salto y modifica el sprite del personaje según la situación
    private void movSalto()
    {
        if (isJump && isGrounded())
        {
            jumpSoundEffect.Play();
            player.velocity = new Vector2(player.velocity.x, jumpForce);
            isJump = false;

        }

        if (player.velocity.y > 1f || player.velocity.y < -1f)
        {
            state = MovementState.jumping;
        }
    }


    //----------------------------------
    //CONTROL DE CONTACTO CON EL TERRENO
    //----------------------------------


    private bool isGrounded()
    {
        //esto crea una caja alrededor del personaje. La caja estará un poco por debajo del personaje y será esta caja lo que controlaremos
        //esto evita que podamos saltar si estamos tocando el lateral de la caja y no la parte inferior
       return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


    //------------------------------------------------------
    //--------------EXTRA PARA MOVIMIENTO PC----------------
    //------------------------------------------------------
    //Este metodo es para mover al jugador con el teclado y poder probarlo en pc
    private void movimientoPC()
    {
        MovementState state;
        float dirX = Input.GetAxisRaw("Horizontal");

        if (isGrounded())
        {
            player.velocity = new Vector2(dirX * 7f, player.velocity.y);
        }
        else {
            player.velocity = new Vector2(dirX * 5f, player.velocity.y);
        }
       

        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            jumpSoundEffect.Play();
            player.velocity = new Vector2(player.velocity.x, jumpForce);
        }

        if (dirX>0f){
            state = MovementState.walking;
            sprite.flipX = false;
        }
        else if(dirX<0f){
            state = MovementState.walking;
            sprite.flipX = true;
        }
        else{
            state = MovementState.idle;
        }

        if (player.velocity.y > 1f || player.velocity.y<-1f) {
            state = MovementState.jumping;
        }

        anim.SetInteger("State", (int)state);
    }


}

