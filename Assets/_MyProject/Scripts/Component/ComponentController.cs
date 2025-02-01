using UnityEngine;

public class ComponentController : MonoBehaviour
{
    [Header("Unit")]
    [field:SerializeField] public StateManager StateManager;
    [field:SerializeField] public Movement Movement;
    [field:SerializeField] public Jump Jump;
    [field:SerializeField] public Health Health;
    [field:SerializeField] public Attack Skill;
}
