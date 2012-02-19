using System;
using System.Collections.Generic;
using System.Drawing;
using Vortex.Utils.Xml;
using BattleCore;
using BattleCore.Units;
using BattleUI.VisualUnits;

namespace BattleUI
{
    class LevelInfo
    {
        public const char MapBlank = ' ';
        public const char MapMetalWall = '#';
        public const char MapTreeWall = 'T';
        public const char MapPlayerStartPosition = 'S';
        public const char MapEnemyStartPosition = 'E';
        public const char MapBonusStartPosition = 'B';

        enum MapItemType
        {
            Blank,
            MetalWall,
            TreeWall,
            PlayerStartPos,
            EnemyStartPos,
            BonusStartPos,
        };

        public LevelInfo()
        {
            Player = new Point();
            MetalWalls = new List<Point>();
            EnemiesGenerationPoints = new List<Point>();
            TreeWalls = new List<Point>();
            BonusGenerationPoints = new List<Point>();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Point Player { get; private set; }
        public List<Point> MetalWalls { get; private set; }
        public List<Point> TreeWalls { get; private set; }
        public List<Point> EnemiesGenerationPoints { get; private set; }
        public List<Point> BonusGenerationPoints { get; private set; }

        public void Load(int levelNumber)
        {
            XmlDocument document = new XmlDocument("Resources\\levels\\" + levelNumber + ".xml");
            XmlNode levelNode = document.Nodes[0];

            XmlNode mapNode = levelNode.Children.GetElementsByTagName("map")[0];
            Width = Int32.Parse(mapNode.Attributes["width"]);
            Height = Int32.Parse(mapNode.Attributes["height"]);

            m_map = new MapItemType[Width, Height];
            for (int n = 0; n < Height; ++n)
            {
                XmlNode rowNode = mapNode.Children[n];
                FillMapRow(rowNode.Value, n);
            }
        }

        private MapItemType[,] Map
        {
            get { return m_map; }
        }

        private void FillMapRow(string mapRowString, int rowNo)
        {
            int colNo = 0;
            foreach (char ch in mapRowString.ToCharArray())
            {
                MapItemType cellType = CharToCellItemType(ch);
                if (cellType == MapItemType.PlayerStartPos)
                {
                    cellType = MapItemType.Blank;
                    Player = new Point(colNo, rowNo);
                }
                else if (cellType == MapItemType.MetalWall)
                {
                    cellType = MapItemType.Blank;
                    MetalWalls.Add(new Point(colNo, rowNo));
                }
                else if (cellType == MapItemType.EnemyStartPos)
                {
                    EnemiesGenerationPoints.Add(new Point(colNo, rowNo));
                }
                else if (cellType == MapItemType.TreeWall)
                {
                    TreeWalls.Add(new Point(colNo, rowNo));
                }
                else if (cellType == MapItemType.BonusStartPos)
                {
                    BonusGenerationPoints.Add(new Point(colNo, rowNo));
                }
                m_map[colNo++, rowNo] = cellType;
            }
        }

        private MapItemType CharToCellItemType(char ch)
        {
            if (ch == MapBlank)
            {
                return MapItemType.Blank;
            }
            else if (ch == MapMetalWall)
            {
                return MapItemType.MetalWall;
            }
            else if (ch == MapPlayerStartPosition)
            {
                return MapItemType.PlayerStartPos;
            }
            else if (ch == MapEnemyStartPosition)
            {
                return MapItemType.EnemyStartPos;
            }
            else if (ch == MapTreeWall)
            {
                return MapItemType.TreeWall;
            }
            else if (ch == MapBonusStartPosition)
            {
                return MapItemType.BonusStartPos;
            }
            return MapItemType.Blank;
        }
        
        private MapItemType[,] m_map;
    }
}
