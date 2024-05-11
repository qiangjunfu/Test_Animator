using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public class Patrol__2 : NavMeshMovement
    {
        [Tooltip("Should the agent patrol the waypoints randomly?")]
        [UnityEngine.Serialization.FormerlySerializedAs("randomPatrol")]
        public SharedBool m_RandomPatrol;
        [Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
        [UnityEngine.Serialization.FormerlySerializedAs("waypointPauseDuration")]
        public SharedFloat m_WaypointPauseDuration;
        [Tooltip("The waypoints to move to")]
        [UnityEngine.Serialization.FormerlySerializedAs("waypoints")]
        public SharedGameObjectList m_Waypoints;

        // The current index that we are heading towards within the waypoints array
        private int m_WaypointIndex;
        private float m_WaypointReachedTime;



        public enum NPCState
        {
            Patrol,
            Stay
        }

        private NPCState currentState;
        private bool stateChanged = true; // 标志位，用来确保状态改变只打印一次




        public override void OnStart()
        {
            base.OnStart();

            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < m_Waypoints.Value.Count; ++i)
            {
                if ((localDistance = Vector3.Magnitude(transform.position - m_Waypoints.Value[i].transform.position)) < distance)
                {
                    distance = localDistance;
                    m_WaypointIndex = i;
                }
            }
            m_WaypointReachedTime = -1;
            SetDestination(Target());


            currentState = NPCState.Patrol;
            Debug.LogFormat("开始巡逻:  " + currentState);
            int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
            MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 2, id);
        }

        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (m_Waypoints.Value.Count == 0)
            {
                return TaskStatus.Failure;
            }

            // Check if the NPC has arrived at the waypoint
            if (HasArrived())
            {
                if (currentState != NPCState.Stay)
                {
                    currentState = NPCState.Stay;
                    stateChanged = true; // Set state changed
                }

                if (m_WaypointReachedTime == -1)
                {
                    m_WaypointReachedTime = Time.time;
                }

                // Wait the required duration before switching waypoints
                if (m_WaypointReachedTime + m_WaypointPauseDuration.Value <= Time.time)
                {
                    NextWaypoint(); // Method to handle waypoint switching
                    currentState = NPCState.Patrol;
                    stateChanged = true; // Set state changed
                    m_WaypointReachedTime = -1;
                }
            }

            if (stateChanged)
            {
                Debug.Log("巡逻状态改变: " + currentState);
                stateChanged = false; // Reset the state changed flag

                int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
                switch (currentState)
                {
                    case NPCState.Patrol:
                        MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 2, id);
                        break;
                    case NPCState.Stay:
                        MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 1, id);
                        break;
                    default:
                        break;
                }
            }


            return TaskStatus.Running;
        }

        private void NextWaypoint()
        {
            if (m_RandomPatrol.Value)
            {
                if (m_Waypoints.Value.Count == 1)
                {
                    m_WaypointIndex = 0;
                }
                else
                {
                    var newWaypointIndex = m_WaypointIndex;
                    while (newWaypointIndex == m_WaypointIndex)
                    {
                        newWaypointIndex = Random.Range(0, m_Waypoints.Value.Count);
                    }
                    m_WaypointIndex = newWaypointIndex;
                }
            }
            else
            {
                m_WaypointIndex = (m_WaypointIndex + 1) % m_Waypoints.Value.Count;
            }
            SetDestination(Target());
        }




        // Return the current waypoint index position
        private Vector3 Target()
        {
            if (m_WaypointIndex >= m_Waypoints.Value.Count)
            {
                return transform.position;
            }
            return m_Waypoints.Value[m_WaypointIndex].transform.position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            m_RandomPatrol = false;
            m_WaypointPauseDuration = 0;
            m_Waypoints = null;

            currentState = NPCState.Patrol; // Default state
            stateChanged = true;
        }



        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (m_Waypoints == null || m_Waypoints.Value == null)
            {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < m_Waypoints.Value.Count; ++i)
            {
                if (m_Waypoints.Value[i] != null)
                {
                    UnityEditor.Handles.SphereHandleCap(0, m_Waypoints.Value[i].transform.position, m_Waypoints.Value[i].transform.rotation, 1, EventType.Repaint);
                }
            }
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}