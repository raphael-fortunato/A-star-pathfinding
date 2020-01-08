using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AStar_Pathfinding
{
    public partial class AStar
    {


        static Random r = new Random();
        List<double> gscore, fscore;
        List<Point> nodes;
        List<int>[] neighbours;
        int[] parents;

        public AStar(List<Point> nodes, List<int>[] neighbours)
        {
            this.nodes = nodes;
            this.neighbours = neighbours;
            gscore = new List<double>();
            fscore = new List<double>();
            for (int i = 0; i < nodes.Count; i++)
            {
                gscore.Add(double.MaxValue);
                fscore.Add(double.MaxValue);
            }
            parents = new int[nodes.Count];
            InitParent();

        }

        public bool CalcPath(Point start, Point end)
        {

            List<int> openset = new List<int>();
            List<int> closedset = new List<int>();
            int first = IndexOf(this.nodes, start);
            openset.Add(first);
            this.fscore[first] = this.CalcDistance(start, end);
            this.gscore[first] = 0;
            int current;
            while (openset.Any())
            {
                current = LowestfScore(openset, end);

                if (Equals(this.nodes[current], end))
                {
                    return true;
                }

                openset.Remove(current);
                for (int i = 0; i < this.neighbours[current].Count; i++)
                {

                    int neighbour = this.neighbours[current][i];

                    if (closedset.Contains(neighbour)) {
                        continue;
                    }
                    double tentative_cost = this.gscore[current] + CalcDistance(this.nodes[current], this.nodes[neighbour]);

                    if (!openset.Contains(neighbour)) {
                        openset.Add(neighbour);
                    }
                    if (tentative_cost >= this.gscore[neighbour])
                    {
                        continue;
                    }
                    else
                    {
                        this.gscore[neighbour] = tentative_cost;
                        this.fscore[neighbour] = this.gscore[neighbour] + this.CalcDistance(this.nodes[neighbour], end);
                        this.parents[neighbour] = current;
                    }
                }
                closedset.Add(current);
            }
            Console.WriteLine("Failed to find path!");
            return false;
        }

        public List<Point> FindPath(Point start, Point end)
        {
            List<Point> path = new List<Point>();
            bool succeed = CalcPath(start, end);
            if (!succeed)
            {
                path.Add(start);
                return path;
            }
            int index;
            Point current = end;
            path.Add(end);


            while (!Equals(current, start))
            {
                index = this.parents[IndexOf(this.nodes, current)];
                current = this.nodes[index];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
        void InitParent()
        {
            for(int i = 0; i < parents.Length; i++)
            {
                parents[i] = -1;
            }
        }

        static void Main()
        {
            //AStar a = new AStar();
        }

        int LowestfScore(List<int> set, Point end)
        {

            int index = 0;
            for (int i = 0; i < set.Count; i++)
            {

                if (i == 0)
                {
                    index = set[0];
                    continue;
                }
                if (this.gscore[set[i]] + this.CalcDistance(this.nodes[set[i]], end) < this.gscore[index] + this.CalcDistance(this.nodes[index], end))
                {
                    index = set[i];
                }

            }
            return index;
        }


        double CalcDistance(Point a, Point b)
        {
            double distX = (a.X - b.X) * (a.X - b.X);
            double distY = (a.Y - b.Y) * (a.Y - b.Y);
            double distance = Math.Sqrt(distX + distY);
            return distance;

        }

        int IndexOf(List<Point> l, Point p)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (p.X == l[i].X && p.Y == l[i].Y)
                {
                    return i;
                }
            }
            throw new NullReferenceException("index not found!");
        }

        public void Reset()
        {
            this.gscore = new List<double>();
            this.fscore = new List<double>();
            for (int i = 0; i < this.nodes.Count; i++)
            {
                this.gscore.Add(double.MaxValue);
                this.fscore.Add(double.MaxValue);
            }
            this.parents = new int[nodes.Count];

        }
        public List<Point> VisitedNodes()
        {
            List<Point> visited = new List<Point>();
            foreach(int parent in parents)
            {
                if(parent != -1)
                {
                    visited.Add(this.nodes[parent]);
                }
            }
            return visited;
        } 

        public int[] Parents{get{return this.parents; } }









    }
}
