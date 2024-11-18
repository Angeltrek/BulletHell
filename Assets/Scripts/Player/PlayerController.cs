using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables públicas
    public float moveSpeed = 5f;           // Velocidad base de movimiento horizontal
    public float runSpeed = 8f;            // Velocidad al correr
    public float jumpForce = 5f;           // Fuerza del salto
    public LayerMask groundLayer;         // Capa del suelo para detectar colisiones
    public Transform groundCheck;          // Punto para verificar si está en el suelo
    public float groundCheckRadius = 0.2f; // Radio para detectar el suelo
    public float fallMultiplier = 2.5f;    // Multiplicador de gravedad al caer
    public float lowJumpMultiplier = 2f;   // Multiplicador para saltos pequeños
    private AudioManager audioManager;  // Referencia al AudioManager

    // Variables privadas
    private Rigidbody2D rb;                // Referencia al Rigidbody2D del personaje
    private Animator animator;             // Referencia al Animator
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private bool isGrounded;               // Si el personaje está en el suelo
    private bool isRunning;                // Si el personaje está corriendo
    private bool canMove = true;           // Control para si el jugador puede moverse

    // Variables para controlar la frecuencia del sonido
    private float soundCooldown = 0f;      // Temporizador para controlar la frecuencia de los sonidos
    public float runSoundInterval = 1f;  // Intervalo de sonido al correr
    public float walkSoundInterval = 1f; // Intervalo de sonido al caminar

    // Start se llama al inicio
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioManager = FindObjectOfType<AudioManager>();  // Obtener el AudioManager

        if (audioManager == null)
        {
            Debug.LogError("AudioManager no encontrado.");
        }
    }

    // Update se llama una vez por frame
    void Update()
    {
        if (canMove) // Solo permitir movimiento si canMove es verdadero
        {
            // Movimiento horizontal
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            // Detectar si está corriendo (manteniendo presionada la tecla Shift)
            isRunning = Input.GetKey(KeyCode.LeftShift);

            // Actualizar animaciones
            animator.SetBool("isMoving", horizontalInput != 0);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isRunning", isRunning);

            // Flip horizontal según la dirección
            if (horizontalInput != 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = horizontalInput < 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }

            // Detectar si está en el suelo
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Controlar la reproducción del sonido
            if (isGrounded)
            {
                if (isRunning)
                {
                    // Si está corriendo, hacer sonar el sonido más rápido
                    soundCooldown -= Time.deltaTime;
                    if (soundCooldown <= 0f)
                    {
                        audioManager.PlayPlayerRunning();
                        soundCooldown = runSoundInterval; // Reiniciar temporizador con intervalo de correr
                    }
                }
                else if (horizontalInput != 0)
                {
                    // Si está caminando (no corriendo), hacer sonar el sonido más despacio
                    soundCooldown -= Time.deltaTime;
                    if (soundCooldown <= 0f)
                    {
                        audioManager.PlayPlayerRunning();
                        soundCooldown = walkSoundInterval; // Reiniciar temporizador con intervalo de caminar
                    }
                }
            }

            // Salto
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // Reproducir sonido de salto
                if (audioManager != null)
                {
                    audioManager.PlayPlayerJump();
                }
            }
        }
    }

    // FixedUpdate se usa para movimiento físico
    void FixedUpdate()
    {
        if (canMove) // Solo permitir movimiento si canMove es verdadero
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            // Elegir velocidad según si está corriendo
            float currentSpeed = isRunning ? runSpeed : moveSpeed;

            // Actualizar la velocidad horizontal
            rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);

            // Aumentar gravedad si el personaje está cayendo
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }

    // Dibujar el área de groundCheck en la vista de Scene para debug
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    // Función para bloquear y desbloquear el movimiento
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
