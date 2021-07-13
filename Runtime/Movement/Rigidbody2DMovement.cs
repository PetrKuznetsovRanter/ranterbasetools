using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DMovement : MonoBehaviour, IDirectionalMovement, ITargetMovement
    {
        #region Events
        /// <summary>
        /// Target point achieved event
        /// </summary>
        public event TargetPointAchievedDelegate TargetPointAchievedEvent;
        #endregion Events
        #region Parameters 
        /// <summary>
        /// Scalar value of velocity
        /// </summary>
        [SerializeField]
        [Range(0, float.MaxValue)]
        float speed;

        /// <summary>
        /// Scalar value of velocity property
        /// </summary>
        /// <value>Scalar value of velocity</value>
        public float Speed { get { return speed; } set { speed = Mathf.Clamp(value, 0, float.MaxValue); Rigidbody2D.velocity = direction.normalized * Speed; } }
        #endregion Parameters 

        #region State
        /// <summary>
        /// Rigidbody2D cache
        /// </summary>
        new Rigidbody2D rigidbody2D;
        /// <summary>
        /// Rigidbody2D cache property
        /// </summary>
        public Rigidbody2D Rigidbody2D
        { get { return rigidbody2D = rigidbody2D ?? GetComponent<Rigidbody2D>(); } }

        /// <summary>
        /// Velocity direction
        /// </summary>
        Vector3 direction;
        /// <summary>
        /// Velocity direction property
        /// </summary>
        public Vector3 Direction
        {
            get { return direction; }
            set
            {
                direction = value; Rigidbody2D.velocity = direction.normalized * Speed; if (dynamicTarget != null)
                {
                    directional = false;
                }
                else directional = true;
            }
        }
        /// <summary>
        ///  Dynamic target for moving an object
        /// </summary>
        Transform dynamicTarget = null;
        /// <summary>
        ///  Get/set a dynamic target for moving an object
        /// </summary>
        /// <value>Static target</value>
        public Transform DynamicTarget
        {
            get { return dynamicTarget; }
            set
            {
                dynamicTarget = value;

                if (dynamicTarget != null)
                {
                    directional = false;
                    StaticTarget = dynamicTarget.position;
                }
                else
                {
                    directional = true;
                }
            }
        }
        /// <summary>
        /// Static target for moving an object
        /// </summary>
        Vector3 staticTarget;
        /// <summary>
        ///  Get/set a static target for moving an object
        /// </summary>
        /// <value>Static target</value>
        public Vector3 StaticTarget
        {
            get { return staticTarget; }
            set { staticTarget = value; Direction = staticTarget - transform.position; }

        }

        /// <summary>
        /// Have target
        /// </summary>
        bool directional = true;
        #endregion State
        #region Methods

        #endregion Methods

        #region Unity
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected void CustomUpdate()
        {
            if (!directional)
            {
                if (DynamicTarget != null)
                {
                    if (StaticTarget != DynamicTarget.position)
                    {
                        StaticTarget = DynamicTarget.position;
                    }
                }
                if ((StaticTarget - transform.position).sqrMagnitude <= Speed * Speed)
                {
                    transform.position = StaticTarget;
                    if (TargetPointAchievedEvent != null) TargetPointAchievedEvent();
                }
            }
        }
        #endregion Unity
    }
}