using Models;

namespace Components
{
    public static class GameStateChecker
    {

        public static bool IsKeepGoing = false;
        
        public static GameState CheckGameState(CubeBehaviour[,] cubes, int mapSize)
        {
            int maxValue = 0;
            bool isLost = true;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (cubes[i, j] is null)
                        continue;

                    CubeBehaviour cube = cubes[i, j];

                    if (cube.CubeScore > maxValue)
                        maxValue = cube.CubeScore;

                    if (_isCubeMovable(cubes, cube, i, j, mapSize))
                        isLost = false;
                }
            }

            if (maxValue >= 2048 && !IsKeepGoing)
                return GameState.Win;
            if (isLost)
                return GameState.Lose;
            return GameState.Play;
        }

        private static bool _isCubeMovable(CubeBehaviour[,] cubes, CubeBehaviour cube, int xCoord, int zCoord, int mapSize)
        {

            if (xCoord - 1 >= 0)
            {
                if (cubes[xCoord - 1, zCoord] is null || cube.CubeScore == cubes[xCoord - 1, zCoord].CubeScore)
                    return true;
            }

            if (zCoord - 1 >= 0)
            {
                if (cubes[xCoord, zCoord - 1] is null || cube.CubeScore == cubes[xCoord, zCoord - 1].CubeScore)
                    return true;
            }

            if (zCoord + 1 < mapSize)
            {
                if (cubes[xCoord, zCoord + 1] is null || cube.CubeScore == cubes[xCoord, zCoord + 1].CubeScore)
                    return true;
            }

            if (xCoord + 1 < mapSize)
            {
                if (cubes[xCoord + 1, zCoord] is null || cube.CubeScore == cubes[xCoord + 1, zCoord].CubeScore)
                    return true;
            }

            return false;
        }
    }
}