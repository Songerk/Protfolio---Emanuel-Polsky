using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeMechanics
{

    public interface IUiLife
    {
        public void UpdateUi(float currentLife, float MaxLife, bool addLife);

    }
    public interface IDeath
    {
        public void Die();
    }
}
