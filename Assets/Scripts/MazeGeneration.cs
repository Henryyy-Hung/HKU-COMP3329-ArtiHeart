using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGeneration : MonoBehaviour
{
    public MazeCell _mazeCellPrefab;

    public int _mazeWidth;

    public int _mazeDepth;

    public MazeCell[,] _mazeGrid;
    public GameObject monsterPrefab1;
    public GameObject monsterPrefab2;

    public GameObject keyPrefab;
    public GameObject chestPrefab;

    //private AudioSource bgaudio;






    // Start is called before the first frame update
    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeDepth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }

        }
        GenerateMaze(null, _mazeGrid[0, 0]);

        placeMonster();

        PlaceKeysAndChest();

        //bgaudio = GetComponent<AudioSource>();
        //bgaudio.Play();


    }


    void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        clearWalls(previousCell, currentCell);


        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);


    }
    MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;
        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                //return but not exit the method
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }


        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }

        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }


    void clearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }


    }
    // Update is called once per frame
    void Update()
    {

    }


    //--------------------------------------------------------------------
    void placeMonster()
    {
        GameObject monster1 = Instantiate(monsterPrefab1, _mazeGrid[0, _mazeDepth - 1].transform.position, Quaternion.identity);
        GameObject monster2 = Instantiate(monsterPrefab2, _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position, Quaternion.identity);
        // monster1.GetComponent<MonsterAI>().findPath(_mazeGrid[0,_mazeDepth-1], _mazeWidth, _mazeDepth, _mazeGrid);
        // monster2.GetComponent<MonsterAI>().findPath(_mazeGrid[_mazeWidth-1,_mazeDepth-1], _mazeWidth, _mazeDepth, _mazeGrid);
    }

    void PlaceKeysAndChest()
    {
        // 随机选择放置两个钥匙的单元格
        PlaceObjectInRandomCell(keyPrefab, 2); // 假设你有一个keyPrefab

        // 随机选择放置宝箱的单元格
        PlaceObjectInRandomCell(chestPrefab, 1); // 假设你有一个chestPrefab
    }

    void PlaceObjectInRandomCell(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(0, _mazeWidth);
            int z = Random.Range(0, _mazeDepth);

            // 确保选择的单元格尚未放置对象
            // Correct condition
            while (_mazeGrid[x, z].getKey() || _mazeGrid[x, z].getChest())

            {
                x = Random.Range(3, _mazeWidth);
                z = Random.Range(1, _mazeDepth);
            }

            Instantiate(prefab, _mazeGrid[x, z].transform.position, Quaternion.identity, _mazeGrid[x, z].transform);

            if (prefab == keyPrefab)
            {
                _mazeGrid[x, z].setKey();
            }
            else if (prefab == chestPrefab)
            {
                _mazeGrid[x, z].setChest();

            }
        }
    }
}
