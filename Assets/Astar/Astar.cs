using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    private const int mapWith = 15;
    private const int mapHeight = 15;

    private Point[,] map = new Point[mapWith, mapHeight];
    // Start is called before the first frame update
    void Start()
    {
        InitMap();
        Point start = map[2, 3];
        Point end = map[7, 4];
        FindPath(start, end);
        ShowPath(start, end);
    }
    private void ShowPath(Point start, Point end)
    {
        Point temp = end;
        while (true)
        {           
            Color c = Color.gray;
            if (temp == start)
            {
                c = Color.green;
            }
            else if (temp == end)
            {
                c = Color.red;
            }
            CreateCube(temp.X, temp.Y, c);

            if (temp.Parent == null)
                break;
            temp = temp.Parent;
        }
        for (int x = 0; x < mapWith; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y].IsWall)
                {
                    CreateCube(x, y, Color.blue);
                }
            }
        }
    }
    private void CreateCube(int x, int y, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(x, y, 0);
        go.GetComponent<Renderer>().material.color = color;
    }
    private void InitMap()
    {
        for (int x = 0; x < mapWith; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = new Point(x, y);
            }
        }
        map[4, 2].IsWall = true;
    }
    private void FindPath(Point start, Point end)
    {
        List<Point> openList = new List<Point>();
        List<Point> closeList = new List<Point>();
        openList.Add(start);
        while (openList.Count > 0)
        {
            Point point = FindMinFOfPoint(openList);
            openList.Remove(point);
            closeList.Add(point);
            List<Point> surroundPoints = GetSurroundPoints(point);
            PointsFilter(surroundPoints, closeList);
            foreach (Point surroundPoint in surroundPoints)
            {
                if (openList.IndexOf(surroundPoint) > -1)
                {
                    float nowG = CalcG(surroundPoint, point);
                    if (nowG < surroundPoint.G)
                    {
                        surroundPoint.UpdateParent(point, nowG);
                    }
                }
                else
                {
                    surroundPoint.Parent = point;
                    CalcF(surroundPoint, end);
                    openList.Add(surroundPoint);
                }
            }
            //判断一下
            if (openList.IndexOf(end) > -1)
            {
                break;
            }
        }

    }
    private float CalcG(Point now, Point parent)
    {
        return Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(parent.X, parent.Y)) + parent.G;
    }
    private void PointsFilter(List<Point> src, List<Point> closeList)
    {
        foreach (Point p in closeList)
        {
            if (src.IndexOf(p) > -1)
            {
                src.Remove(p);
            }
        }
    }
    private List<Point> GetSurroundPoints(Point point)
    {
        Point up = null, down = null, left = null, right = null;
        Point lu = null, ru = null, ld = null, rd = null;//left-up;right-up;left-down;right-down
        if (point.Y < mapHeight - 1)
        {
            up = map[point.X, point.Y + 1];
        }
        if (point.Y > 0)
        {
            down = map[point.X, point.Y - 1];
        }
        if (point.X > 0)
        {
            left = map[point.X - 1, point.Y];
        }
        if (point.X < mapWith - 1)
        {
            right = map[point.X + 1, point.Y];
        }
        if (up != null && left != null)
        {
            lu = map[point.X - 1, point.Y + 1];
        }
        if (up != null && right != null)
        {
            ru = map[point.X + 1, point.Y + 1];
        }
        if (down != null && left != null)
        {
            ld = map[point.X - 1, point.Y - 1];
        }
        if (down != null && right != null)
        {
            rd = map[point.X + 1, point.Y - 1];
        }
        List<Point> list = new List<Point>();
        if (down != null && down.IsWall == false)
        {
            list.Add(down);
        }
        if (up != null && up.IsWall == false)
        {
            list.Add(up);
        }
        if (left != null && left.IsWall == false)
        {
            list.Add(left);
        }
        if (right != null && right.IsWall == false)
        {
            list.Add(right);
        }
        if (lu != null && lu.IsWall == false && left.IsWall == false && up.IsWall == false)
        {
            list.Add(lu);
        }
        if (ld != null && ld.IsWall == false && left.IsWall == false && down.IsWall == false)
        {
            list.Add(ld);
        }
        if (ru != null && ru.IsWall == false && right.IsWall == false && up.IsWall == false)
        {
            list.Add(ru);
        }
        if (rd != null && rd.IsWall == false && right.IsWall == false && down.IsWall == false)
        {
            list.Add(rd);
        }
        return list;
    }
    private void CalcF(Point now, Point end)
    {
        
        float h = Mathf.Abs(end.X - now.X) + Mathf.Abs(end.Y - now.Y);
        float g = 0;
        if (now.Parent == null)
        {
            g = 0;
        }
        else
        {
            g = Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(now.Parent.X, now.Parent.Y)) + now.Parent.G;
        }
        float f = g + h;
        now.F = f;
        now.G = g;
        now.H = h;
    }
    private Point FindMinFOfPoint(List<Point> openList)
    {
        float f = float.MaxValue;
        Point temp = null;
        foreach (Point p in openList)
        {
            if (p.F < f)
            {
                temp = p;
                f = p.F;
            }
        }
        return temp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
