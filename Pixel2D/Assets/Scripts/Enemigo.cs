using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float vida;
    public float daņoAlJugador;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TomarDamage(float damage)
    {
        vida -= damage;
        animator.SetTrigger("RecibirDaņo");

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        animator.SetTrigger("Muerte");
        Destroy(gameObject, 1f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CombateJugador>().TomarDaņo(daņoAlJugador, other.GetContact(0).normal);

        }
    }

}
