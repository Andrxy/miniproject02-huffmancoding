using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace miniproject02_huffmancoding.Entities
{
    public class HuffmanNode : IComparable<HuffmanNode>
    {
        public char? Char {  get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }

        public HuffmanNode(char? Char, int Frequency)
        {
            this.Char = Char;
            this.Frequency = Frequency;
            this.Left = null;
            this.Right = null;
        }

        public bool IsLeaf()
        {
            return Left == null && Right == null;
        }
        public int CompareTo(HuffmanNode other)
        {
            return Frequency.CompareTo(other.Frequency);
        }
    }
}
