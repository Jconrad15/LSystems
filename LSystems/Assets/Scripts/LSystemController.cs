using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSystems
{
    public class LSystemController : MonoBehaviour
    {
        [SerializeField]
        private PlantGenerator plantGen;

        private GameObject createdPlant;

        private bool isCoroutine = true;

        // Start is called before the first frame update
        void Start()
        {
            GenerateNewPlant();
        }

        public void GenerateNewPlant()
        {
            plantGen.StopAllCoroutines();
            Destroy(createdPlant);

            createdPlant = plantGen.CreatePlant(isCoroutine);
        }

        public void SetIsCoroutine(bool newIsCoroutine)
        {
            isCoroutine = newIsCoroutine;
        }



    }
}