using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity
{
    public class Level
    {
        XmlReader _reader;
        ObjectCollection _objectCollection;
        Player player;

        public Level(StreamReader file)
        {
            _reader = XmlReader.Create(file);
            _objectCollection = new ObjectCollection();
        }

        public Player P
        {
            get { return player; }
        }

        public ObjectCollection OC
        {
            get { return _objectCollection; }
        }

        public void Load()
        {
            // skip past initial element
            while (_reader.NodeType != XmlNodeType.Element || _reader.Name != "level")
                _reader.Read();

            while (!_reader.EOF)
            {
                if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "player")
                {
                    _reader.MoveToAttribute("x");
                    float x = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("y");
                    float y = Tiling.ToPixels(float.Parse(_reader.Value));
                    player = new Player(new Vector2(x, y));
                    _objectCollection.Add(player);
                }
                else if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "box")
                {
                    _reader.MoveToAttribute("x");
                    float x = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("y");
                    float y = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("width");
                    float w = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("height");
                    float h = Tiling.ToPixels(float.Parse(_reader.Value));
                    Box b = new Box(new RectF(x, y, w, h));
                    _objectCollection.Add(b);
                }
                else if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "floor")
                {
                    _reader.MoveToAttribute("x");
                    float x = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("y");
                    float y = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("width");
                    float w = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("height");
                    float h = Tiling.ToPixels(float.Parse(_reader.Value));
                    FloorSegment f = new FloorSegment(new RectF(x, y, w, h));
                    _objectCollection.Add(f);
                }
                else if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "laser")
                {
                    _reader.MoveToAttribute("x");
                    float x = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("y");
                    float y = Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("direction");
                    LaserDirection ld = (LaserDirection)_reader.Value[0];
                    Laser laser = new Laser(new Vector2(x, y), ld);
                    _objectCollection.Add(laser);
                }
                else if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "win")
                {
                    _reader.MoveToAttribute("x");
                    int x = (int)Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("y");
                    int y = (int)Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("width");
                    int w = (int)Tiling.ToPixels(float.Parse(_reader.Value));
                    _reader.MoveToAttribute("height");
                    int h = (int)Tiling.ToPixels(float.Parse(_reader.Value));
                    WinArea winArea = new WinArea(new Rectangle(x, y, w, h));
                    _objectCollection.WinArea = winArea;
                }
                else if (_reader.NodeType == XmlNodeType.EndElement)
                {
                    _reader.ReadEndElement();
                    _reader.Close();
                    return;
                }
                else
                    _reader.Read();
            }
        }
    }
}
