using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PairFinder
{
    public class RecordParticles : MonoBehaviour
    {
        [SerializeField] private List<GameObject> particlesForRecord;

        private void OnEnable()
        {
            foreach(GameObject obj in particlesForRecord)
            {
                obj.SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (GameObject obj in particlesForRecord)
            {
                obj.SetActive(false);
            }
        }

    }
}
