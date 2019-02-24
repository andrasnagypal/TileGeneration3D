using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public interface ShowPath
    {
        void TurnToColor();
        void TurnBackToOriginalColor();
        int BeginAndEndPath();
    }
}
