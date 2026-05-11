using UnityEngine;

public class BotController : MonoBehaviour
{
    public BotMovement movement;
    public BotNeeds needs;
    public BotBrain brain;
    public BotActions actions;
    public BotVisual visual;

    public BotStateMachine stateMachine;

    [HideInInspector] public Seat targetSeat;

    private void Awake()
    {
        movement = GetComponent<BotMovement>();
        needs = GetComponent<BotNeeds>();
        brain = GetComponent<BotBrain>();
        actions = GetComponent<BotActions>();
        visual = GetComponent<BotVisual>();

        stateMachine = new BotStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(new BotThinkState());
    }

    private void Update()
    {
        stateMachine.Update();
    }
}