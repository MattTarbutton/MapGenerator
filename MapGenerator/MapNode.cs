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
    public class MapNode : INotifyPropertyChanged
    {
        public List<Connection> Connections;
        private Point _position;
        [Browsable(false)]
        public Point Position
        {
            get { return _position; }
            set
            {
                if (value == _position)
                    return;

                _position = value;
                OnPropertyChanged();
                OnPositionChanged();
            }
        }
        
        private Point _truePosition;
        /// <summary>
        /// True position is the position in map units.
        /// </summary>
        [Browsable(false)]
        public Point TruePosition
        {
            get { return _truePosition; }
            set
            {
                if (value == _truePosition)
                    return;

                _truePosition = value;
                _perturbedPosition = value;
                OnPropertyChanged();
            }
        }
        private Point _perturbedPosition;
        /// <summary>
        /// Perturbed position is the position in map units after being perturbed for the final level.
        /// </summary>
        [Browsable(false)]
        public Point PerturbedPosition
        {
            get { return _perturbedPosition; }
            set
            {
                if (value == _perturbedPosition)
                    return;

                _perturbedPosition = value;
                OnPropertyChanged();
            }
        }
        [Category("Position")]
        [DisplayName("X")]
        [Browsable(false)]
        public int XPosition
        {
            get { return _position.X; }
            set
            {
                if (value == _position.X)
                    return;

                _position.X = value;
                OnPropertyChanged();
                OnPositionChanged();
            }
        }
        [Category("Position")]
        [DisplayName("Y")]
        [Browsable(false)]
        public int YPosition
        {
            get { return _position.Y; }
            set
            {
                if (value == _position.Y)
                    return;

                _position.Y = value;
                OnPropertyChanged();
                OnPositionChanged();
            }
        }
        private RoomType _room;
        [Category("Room")]
        [DisplayName("Room")]
        [Description("The skeleton of the room represented by a letter than most closely represents its shape.")]
        public RoomType Room
        {
            get { return _room; }
            set
            {
                if (value == _room)
                    return;

                _room = value;
                OnPropertyChanged();
            }
        }

        private int _roomSize;
        [Category("Room")]
        [DisplayName("Room Size")]
        [Description("Relative room size in cells, the actual room may be smaller or larger.")]
        public int RoomSize
        {
            get { return _roomSize; }
            set
            {
                if (value == _roomSize)
                    return;

                _roomSize = value;
                OnPropertyChanged();
            }
        }

        private RotationType _roomRotationType;
        [Category("Room")]
        [DisplayName("Room Rotation Type")]
        [Description("Specifiy random rotation or custom rotation.")]
        public RotationType RoomRotationType
        {
            get { return _roomRotationType; }
            set
            {
                if (value == _roomRotationType)
                    return;

                _roomRotationType = value;
                OnPropertyChanged();
            }
        }

        private int _roomRotation;
        [Category("Room")]
        [DisplayName("Room Rotation")]
        [Description("Rotation of the room in degrees.")]
        public int RoomRotation
        {
            get { return _roomRotation; }
            set
            {
                if (value == _roomRotation)
                    return;

                _roomRotation = value;
                OnPropertyChanged();
            }
        }

        private int _pathWidth;
        [Category("Room")]
        [DisplayName("PathWidth")]
        [Description("Relative path width in cells, the actual path width may be smaller or larger.")]
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

        private int _maxPerturb;
        [Category("Room")]
        [DisplayName("Max Perturb Amount")]
        [Description("The position of the node is randomly adjusted within the range given by this value.")]
        public int MaxPerturb
        {
            get { return _maxPerturb; }
            set
            {
                if (value == _maxPerturb)
                    return;
                
                _maxPerturb = value;
                OnPropertyChanged();
            }
        }

        //private int _chanceToStartAlive;
        //[Category("Randomizer")]
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PositionChanged;

        public enum RoomType
        {
            NoRoom,
            RandomRoom,
            CRoom,
            IRoom,
            LRoom,
            ORoom,
            SolidORoom,
            XRoom
        }

        public enum RotationType
        {
            Random,
            Custom
        }

        public MapNode(int x, int y, double positionRatio)
        {
            Connections = new List<Connection>();

            _position = new Point(x, y);
            _truePosition = new Point((int)(x / positionRatio), (int)(y / positionRatio));
            //_chanceToStartAlive = 45;
            _maxPerturb = 0;
            _pathWidth = 10;
            _roomSize = 15;
            _room = RoomType.RandomRoom;
        }

        public MapNode(double positionRatio, string[] csvValues, ref int startIndex)
        {
            Connections = new List<Connection>();

            int.TryParse(csvValues[startIndex], out int x);
            startIndex++;
            x += 20;
            int.TryParse(csvValues[startIndex], out int y);
            startIndex++;
            y += 20;
            _position = new Point(x, y);
            _truePosition = new Point((int)(x / positionRatio), (int)(y / positionRatio));
            Enum.TryParse(csvValues[startIndex], out _room);
            startIndex++;
            int.TryParse(csvValues[startIndex], out _roomSize);
            startIndex++;
            Enum.TryParse(csvValues[startIndex], out _roomRotationType);
            startIndex++;
            int.TryParse(csvValues[startIndex], out _roomRotation);
            startIndex++;
            int.TryParse(csvValues[startIndex], out _pathWidth);
            startIndex++;
            int.TryParse(csvValues[startIndex], out _maxPerturb);
        }

        public string GetCSV()
        {
            string csv = "";
            csv += Position.X + "," + Position.Y + "," + Room.ToString() + "," + RoomSize + "," + RoomRotationType.ToString() + "," + RoomRotation + "," + PathWidth + "," + MaxPerturb;

            return csv;
        }

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPositionChanged([CallerMemberName] String propertyName = "")
        {
            PositionChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPositionChanged()
        {
            PositionChanged?.Invoke(this, new PropertyChangedEventArgs("Position"));
        }
    }
}
