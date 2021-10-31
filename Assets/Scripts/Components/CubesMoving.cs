using System;
using Models;
using UnityEngine;

namespace Components
{
    public static class CubesMoving
    {
        private static int _mapSize;
        private static float _moveDistance;
        private static float _animationTime;
        private static GameObject[,] _cubes;
        private static CubeBehaviour[,] _cubeBehaviours;
        private static MovementDirection _direction;
        
        
        public static bool MoveCells(MovementDirection direction, GameObject[,] cubes, CubeBehaviour[,] cubeBehaviours, int mapSize, float animationTime, float moveDistance,ref int mergedScore)
        {
            if (cubes is null || cubeBehaviours is null)
                throw new NullReferenceException("Cube array is null");

            _direction = direction;
            _mapSize = mapSize;
            _cubes = cubes;
            _cubeBehaviours = cubeBehaviours;
            _animationTime = animationTime;
            _moveDistance = moveDistance;

            bool isAnyCubeMoved = false;

            if (_direction == MovementDirection.Bottom)
            {
                for (int i = _mapSize - 2; i >= 0; i--)
                {
                    for (int j = 0; j < _mapSize; j++)
                    {
                        bool tmp = MoveCell(_cubes[i, j], i, j, ref mergedScore);
                        if (tmp && !isAnyCubeMoved)
                            isAnyCubeMoved = tmp;
                    }
                }
            }

            if (_direction == MovementDirection.Left)
            {
                for (int i = 1; i < _mapSize; i++)
                {
                    for (int j = 0; j < _mapSize; j++)
                    {
                        bool tmp = MoveCell(_cubes[j, i],j, i, ref mergedScore);
                        if (tmp && !isAnyCubeMoved)
                            isAnyCubeMoved = tmp;
                    }
                }


            }

            if (_direction == MovementDirection.Right)
            {
                for (int i = _mapSize - 2; i >= 0; i--)
                {
                    for (int j = 0; j < _mapSize; j++)
                    {
                        bool tmp = MoveCell(_cubes[j, i], j, i, ref mergedScore);
                        if (tmp && !isAnyCubeMoved)
                            isAnyCubeMoved = tmp;
                    }
                }
            }

            if (_direction == MovementDirection.Top)
            {
                for (int i = 1; i < _mapSize; i++)
                {
                    for (int j = 0; j < _mapSize; j++)
                    {
                        bool tmp = MoveCell(_cubes[i, j], i, j, ref mergedScore);
                        if (tmp && !isAnyCubeMoved)
                            isAnyCubeMoved = tmp;
                    }
                }
            }

            return isAnyCubeMoved;
        }
        
        private static void MergeCubes(CubeBehaviour cubeBehaviour1, GameObject cube2, ref int mergedScore)
        {
            mergedScore += cubeBehaviour1.CubeScore;
            
            cubeBehaviour1.IncreaseScore();
            cubeBehaviour1.CubesToDestroy.Add(cube2);
            cubeBehaviour1.IsMerged = true;
        }

        private static bool MoveCell(GameObject cube, int xCoord, int zCoord, ref int mergedScore)
        {
            if (cube is null)
                return false;

            bool isCubeMoved = false;

            CubeBehaviour cubeBehaviour = _cubeBehaviours[xCoord, zCoord];
            CubeBehaviour cubeBehaviour2;
            Vector3 position = cubeBehaviour.transform.position;

            if (_direction == MovementDirection.Bottom)
            {
                for (int i = xCoord + 1; i < _mapSize; i++)
                {
                    cubeBehaviour2 = _cubeBehaviours[i, zCoord];

                    bool isMergable =
                        cubeBehaviour2?.CubeScore == cubeBehaviour.CubeScore
                        && !cubeBehaviour.IsMerged
                        && !cubeBehaviour2.IsMerged;

                    if (_cubes[i, zCoord] is null || isMergable)
                    {
                        if (isMergable)
                        {
                            MergeCubes(cubeBehaviour, _cubes[i, zCoord], ref mergedScore);
                        }
                        
                        isCubeMoved = CalculateMoving(ref xCoord, ref zCoord, ref position);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (_direction == MovementDirection.Top)
            {
                for (int i = xCoord - 1; i >= 0; i--)
                {
                    cubeBehaviour2 = _cubeBehaviours[i, zCoord];

                    bool isMergable =
                        cubeBehaviour2?.CubeScore == cubeBehaviour.CubeScore
                        && !cubeBehaviour.IsMerged
                        && !cubeBehaviour2.IsMerged;

                    if (_cubes[i, zCoord] is null || isMergable)
                    {
                        if (isMergable)
                        {
                            MergeCubes(cubeBehaviour, _cubes[i, zCoord], ref mergedScore);
                        }
                        
                        isCubeMoved = CalculateMoving(ref xCoord, ref zCoord, ref position);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (_direction == MovementDirection.Left)
            {
                for (int i = zCoord - 1; i >= 0; i--)
                {
                    cubeBehaviour2 = _cubeBehaviours[xCoord, i];

                    bool isMergable =
                        cubeBehaviour2?.CubeScore == cubeBehaviour.CubeScore
                        && !cubeBehaviour.IsMerged
                        && !cubeBehaviour2.IsMerged;

                    if (_cubes[xCoord, i] is null || isMergable)
                    {
                        if (isMergable)
                        {
                            MergeCubes(cubeBehaviour, _cubes[xCoord, i], ref mergedScore);
                        }
                        
                        isCubeMoved = CalculateMoving(ref xCoord, ref zCoord, ref position);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (_direction == MovementDirection.Right)
            {
                for (int i = zCoord + 1; i < _mapSize; i++)
                {
                    cubeBehaviour2 = _cubeBehaviours[xCoord, i];

                    bool isMergable =
                        cubeBehaviour2?.CubeScore == cubeBehaviour.CubeScore
                        && !cubeBehaviour.IsMerged
                        && !cubeBehaviour2.IsMerged;

                    if (_cubes[xCoord, i] is null || isMergable)
                    {
                        if (isMergable)
                        {
                            MergeCubes(cubeBehaviour, _cubes[xCoord, i], ref mergedScore);
                        }
                        
                        isCubeMoved = CalculateMoving(ref xCoord, ref zCoord, ref position);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            cubeBehaviour.TranslateCube(position, _animationTime);
            return isCubeMoved;
        }

        private static bool CalculateMoving(ref int xCoord, ref int zCoord,
            ref Vector3 position)
        {
            if (_direction == MovementDirection.Bottom)
            {
                position += new Vector3(_moveDistance, 0, 0);
                ChangeCubeCell(xCoord, zCoord, xCoord + 1, zCoord);
                xCoord++;
            }

            if (_direction == MovementDirection.Left)
            {
                position -= new Vector3(0, 0, _moveDistance);
                ChangeCubeCell(xCoord, zCoord, xCoord, zCoord - 1);
                zCoord--;
            }

            if (_direction == MovementDirection.Right)
            {
                position += new Vector3(0, 0, _moveDistance);
                ChangeCubeCell(xCoord, zCoord, xCoord, zCoord + 1);
                zCoord++;
            }

            if (_direction == MovementDirection.Top)
            {
                position -= new Vector3(_moveDistance, 0, 0);
                ChangeCubeCell(xCoord, zCoord, xCoord - 1, zCoord);
                xCoord--;
            }

            return true;
        }

        private static void ChangeCubeCell(int oldXCoor, int oldZCoor, int newXCoor, int newZCoor)
        {
            (_cubes[newXCoor, newZCoor], _cubes[oldXCoor, oldZCoor]) = (_cubes[oldXCoor, oldZCoor], null);
            (_cubeBehaviours[newXCoor, newZCoor], _cubeBehaviours[oldXCoor, oldZCoor]) = (_cubeBehaviours[oldXCoor, oldZCoor], null);
        }
    }
}