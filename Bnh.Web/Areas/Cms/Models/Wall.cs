using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;

namespace Cms.Models
{
    public partial class Wall
    {
        public string Title { get; set; }

        public float Width { get; set; }

        public ICollection<Brick> Bricks
        {
            get { return this._bricks ?? (this._bricks = new List<Brick>()); }
            set { this._bricks = value; }
        } ICollection<Brick> _bricks = null;
    }
}
