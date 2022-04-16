using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPlatformer
{
    public class Rectangle
    {
        public string name;

        public float width;
        public float height;

        public Rectangle(string name, float w, float h)
        {
            this.name = name;
            width = w;
            height = h;
        }

        public float GetArea()
        {
            return width * height;
        }
    }
}
