using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class Oven : MonoBehaviour
    {
        private MixData currentMix;

        public void Setup()
        {
            currentMix = null;
        }
    }
}