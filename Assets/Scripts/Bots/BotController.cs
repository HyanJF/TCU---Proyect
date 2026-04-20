using UnityEngine;

public class BotController : MonoBehaviour
{
    private MovementController movement;

    private Vector2 targetDirection;
    private float changeTime = 2f;
    private float timer;

    private bool isAtBar = false;
    private bool isFreeRoaming = false;

    private float roamTime = 5f;
    private float roamTimer;

    private int playerLayer;

    private void Awake()
    {
        movement = GetComponent<MovementController>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Start()
    {
        PickNewDirection();
    }

    private void Update()
    {
        // Si está en barra se queda ahi
        if (isAtBar) return;

        // De chill xD
        if (isFreeRoaming)
        {
            timer += Time.deltaTime;
            roamTimer -= Time.deltaTime;

            if (timer >= changeTime)
            {
                PickNewDirection();
                timer = 0f;
            }

            movement.SetMovement(targetDirection);

            // Para que vuelva a la barra
            if (roamTimer <= 0f)
            {
                isFreeRoaming = false;
            }

            return;
        }

        // Ir a la barra
        Transform barra = BotBlackboard.Instance.barra;

        Vector2 dir = (barra.position - transform.position).normalized;
        movement.SetMovement(dir);

        float distance = Vector2.Distance(transform.position, barra.position);

        if (distance < 1.5f)
        {
            ArriveAtBar();
        }
    }

    void ArriveAtBar()
    {
        isAtBar = true;

        movement.SetMovement(Vector2.zero);

        // Ocupa asiento
        BotBlackboard.Instance.GetRandomFreeSeat();

        // Se registra en el sistema
        BotBlackboard.Instance.RegisterBot(gameObject);

        gameObject.SetActive(false);
    }

    // Cuando el Blackboard lo reactiva
    public void OnReactivated()
    {
        isAtBar = false;
        isFreeRoaming = true;

        roamTimer = roamTime;
        timer = 0f;

        PickNewDirection();
    }

    void PickNewDirection()
    {
        targetDirection = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
            return;

        PickNewDirection();
    }
}