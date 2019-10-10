using UnityEngine;
using System.Collections;

namespace SA
{
    public class Food : MonoBehaviour
    {
        public Node playerNode;

        private void Start()
        {
            this.gameObject.name = "Food";
        }
    }
}
