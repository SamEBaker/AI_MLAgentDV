using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform tpPt;
    [SerializeField] private Material win;
    [SerializeField] private Material lose;
    [SerializeField] private Material normal;
    [SerializeField] private MeshRenderer floor;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.RandomRange(2f, -2f), 0, Random.RandomRange(2f, 8f));
        targetTransform.localPosition = new Vector3(Random.RandomRange(2f, -2f), 0, Random.RandomRange(1.5f, 7.5f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(transform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = 2f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            floor.material = win;
            EndEpisode();
        }
        else if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floor.material = lose;
            EndEpisode();

        }
    }
}
