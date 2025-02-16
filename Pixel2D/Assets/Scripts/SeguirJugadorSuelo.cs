using UnityEngine;

public class SeguirJugadorSuelo : MonoBehaviour
{
    public float radioBusqueda;  // Rango para detectar jugador
    public LayerMask capaJugador; // Capa del Jugador
    public Transform transformJugador;
    public float velocidadMovimiento;
    public float distanciaMaxima;
    public Vector3 puntoInicial;
    public bool mirandoDerecha;
    public Rigidbody2D rb2D;
    public Animator animator;

    public EstadosMovimiento estadoActual; // Estado actual del enemigo

    //Enum con los estados del enemigo
    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo,
    }
    private void Start()
    {
        puntoInicial = transform.position;
    }

    private void Update()
    {
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }

    }

    private void EstadoEsperando()
    {
        // Generamos el circulo (psicion enemigo, radio, capa del jugador)
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);
        //Si colisionamos con jugador....
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo; //Si encuentra al jugador = siguiendo
        }

    }

    private void EstadoSiguiendo()
    {
        animator.SetBool("Corriendo", true);

        //Si es null...
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo; // Vuelve y salte de la ejecucion
            return;
        }
        if (transform.position.x < transformJugador.position.x)
        {
            rb2D.linearVelocity = new Vector2(velocidadMovimiento, rb2D.linearVelocity.y);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(-velocidadMovimiento, rb2D.linearVelocity.y);
        }

        GirarAObjetivo(transformJugador.position);

        //Saber cuando cambiar de estado ( cuando el objeto esta lejos de su posicionIncial o el jugador se aleja demasiado
        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima ||
            Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }

    }

    private void EstadoVolviendo()
    {
        if (transform.position.x < puntoInicial.x)
        {
            rb2D.linearVelocity = new Vector2(velocidadMovimiento, rb2D.linearVelocity.y);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(-velocidadMovimiento, rb2D.linearVelocity.y);
        }

        GirarAObjetivo(puntoInicial);

        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            rb2D.linearVelocity = Vector2.zero;

            animator.SetBool("Corriendo", false);

            estadoActual = EstadosMovimiento.Esperando;
        }
    }

    private void GirarAObjetivo(Vector3 objetivo)
    {
        if (objetivo.x > transform.position.x && !mirandoDerecha)
        {
            Girar();

        }
        else if (objetivo.x < transform.position.x && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    //Pintamos el circulo (visual)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
