using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public enum ConnectionType
    {
        Straight,
        RandomWalk
    }

    public class Connection : INotifyPropertyChanged
    {
        private MapNode[] _nodes;
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PositionChanged;

        private int _pathWidth;
        [Category("Path")]
        [DisplayName("Path Width")]
        [Description("Relative width of the path in cells, the actual path width may be smaller or larger.")]
        public int PathWidth
        {
            get { return _pathWidth; }
            set
            {
                if (value == _pathWidth)
                    return;

                _pathWidth = value;
                OnPropertyChanged();
            }
        }

        private int _perturbAmount;
        [Category("Path")]
        [DisplayName("Max Perturb")]
        [Description("A higher value attemps to allow the path to wander more than a smaller value. Min 5 Max 25.")]
        public int PerturbAmount
        {
            get { return _perturbAmount; }
            set
            {
                if (value == _perturbAmount)
                    return;

                if (value < 5)
                {
                    value = 5;
                }
                else if (value > 25)
                {
                    value = 25;
                }

                _perturbAmount = value;
                OnPropertyChanged();
            }
        }

        //private int _chanceToStartAlive;
        //[Category("Path")]
        //[DisplayName("Initial Seeded %")]
        //public int ChanceToStartAlive
        //{
        //    get { return _chanceToStartAlive; }
        //    set
        //    {
        //        if (value == _chanceToStartAlive)
        //            return;

        //        _chanceToStartAlive = value;
        //        OnPropertyChanged();
        //    }
        //}

        private ConnectionType _pathType;
        [Category("Path")]
        [DisplayName("Path Type")]
        [Description("Force a straight path or allow some wandering.")]
        public ConnectionType PathType
        {
            get { return _pathType; }
            set
            {
                if (value == _pathType)
                    return;

                _pathType = value;
                OnPropertyChanged();
            }
        }

        public Connection(MapNode node1, MapNode node2)
        {
            _nodes = new MapNode[2] { node1, node2 };

            node1.PositionChanged += OnPositionChanged;
            node2.PositionChanged += OnPositionChanged;

            //_chanceToStartAlive = 45;
            _pathWidth = 10;
            _perturbAmount = 20;
            _pathType = ConnectionType.RandomWalk;
        }

        public void ChangeNode(MapNode oldNode, MapNode newNode)
        {
            if (_nodes[0] == oldNode)
            {
                if (oldNode.Connections.Contains(this))
                {
                    oldNode.Connections.Remove(this);
                    oldNode.PositionChanged -= OnPositionChanged;
                }
                _nodes[0] = newNode;
                newNode.PositionChanged += OnPositionChanged;
            }
            else
            {
                if (oldNode.Connections.Contains(this))
                {
                    oldNode.Connections.Remove(this);
                    oldNode.PositionChanged -= OnPositionChanged;
                }
                _nodes[1] = newNode;
                newNode.PositionChanged += OnPositionChanged;
            }
        }

        public Point GetNodeLocation(int index)
        {
            if (index >= 0 && index < _nodes.Length)
            {
                return _nodes[index].Position;
            }
            else
            {
                return new Point(0, 0);
            }
        }

        public Point GetNodeTrueLocation(int index)
        {
            if (index >= 0 && index < _nodes.Length)
            {
                return _nodes[index].TruePosition;
            }
            else
            {
                return new Point(0, 0);
            }
        }

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs args)
        {
            PositionChanged?.Invoke(sender, args);
        }

        public void Dispose()
        {
            foreach (MapNode node in _nodes)
            {
                this.PositionChanged = null;
                node.PositionChanged -= OnPositionChanged;
                node.Connections.Remove(this);
            }
        }
    }
}
