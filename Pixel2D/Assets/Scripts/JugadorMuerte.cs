using UnityEngine;

public class JugadorMuerte : MonoBehaviour
{
    public float vida;
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
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            other.gameObject.GetComponent<CombateJugador>().TomarDaņo(20, other.GetContact(0).normal);

        }
    }
}
