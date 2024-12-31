using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Controllers
{
    public interface IMovementEntityController : IEntityController
    {
        public void MoveTo(Vector3 worldPosition);
        public void StopMoving();
        public void Dash(Vector3 direction, float speed, float distance);
    }
}

