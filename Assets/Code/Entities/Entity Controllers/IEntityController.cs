using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Controllers
{
    public interface IEntityController
    {
        EntityBase p_Owner { get; set; }
        
        public void Initialize(EntityBase ownerEntity);
        public void Update();
    }
}
