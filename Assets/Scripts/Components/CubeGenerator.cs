using UnityEngine;
using System.Collections.Generic;
using Models;

namespace Components
{
    public class CubeGenerator : MonoBehaviour
    {
        public static void GenerateCube(List<int> emptyCells, GameObject[,] cubes, CubeBehaviour[,] cubeBehaviours, GameObject cubePrefab, Transform parentTransform, float stepDistance, int mapSize)
        {
            bool isFour = Random.Range(0, 9) == 8;
            int cell = emptyCells[Random.Range(0, emptyCells.Count)];
            emptyCells.Remove(cell);

            GameObject cubeObject = Instantiate(cubePrefab, parentTransform);
            CubeBehaviour cubeBehaviour = cubeObject.GetComponent<CubeBehaviour>();
            if (isFour)
            {
                cubeBehaviour.IncreaseScore();
                cubeBehaviour.UpdateScoreText();

            }

            int xCoord = cell / mapSize;
            int zCoord = cell % mapSize;
            
            cubes[xCoord, zCoord] = cubeObject;
            cubeBehaviours[xCoord, zCoord] = cubeBehaviour;

            Transform cubeTransform = cubeObject.transform;
            int xCoeff = 0;
            int zCoeff = 0;
            
            switch (xCoord)
            {
                case 0:
                    xCoeff = -3;
                    break;
                case 1:
                    xCoeff = -1;
                    break;
                case 2:
                    xCoeff = 1;
                    break;
                case 3:
                    xCoeff = 3;
                    break;
            }

            switch (zCoord)
            {
                case 0:
                    zCoeff = -3;
                    break;
                case 1:
                    zCoeff = -1;
                    break;
                case 2:
                    zCoeff = 1;
                    break;
                case 3:
                    zCoeff = 3;
                    break;
            }
            
            cubeTransform.localPosition = new Vector3(stepDistance * xCoeff, cubeTransform.localPosition.y, stepDistance * zCoeff);
        }
    }
}