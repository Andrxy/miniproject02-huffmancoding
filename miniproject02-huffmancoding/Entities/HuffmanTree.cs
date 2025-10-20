using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using miniproject02_huffmancoding.Entities;

namespace miniproject02_huffmancoding.DataStructures
{
    public class HuffmanTree
    {
        public HuffmanNode Root { get; set; }
        public Dictionary<char?, string> Codes { get; set; }

        public HuffmanTree() 
        {
            Root = null;
            Codes = new Dictionary<char?, string>();
        }

        public void BuildTree(Dictionary<char?, int> frequencies)
        {
            List<HuffmanNode> nodes = new List<HuffmanNode>();
            foreach (var f in frequencies)
                nodes.Add(new HuffmanNode(f.Key, f.Value));

            if (nodes.Count == 1)
            {
                Root = nodes[0];
                Codes[Root.Char] = "0";
                return;
            }

            while (nodes.Count > 1)
            {
                nodes.Sort();

                HuffmanNode nodeOne = nodes[0];
                nodes.RemoveAt(0);

                HuffmanNode nodeTwo = nodes[0];
                nodes.RemoveAt(0);

                HuffmanNode newNode = new HuffmanNode(null, nodeOne.Frequency + nodeTwo.Frequency);
                newNode.Left = nodeOne;
                newNode.Right = nodeTwo;

                nodes.Add(newNode);
            }

            Root = nodes[0];
            GenerateCodes(Root);
        }

        private void GenerateCodes(HuffmanNode current, string code = "")
        {
            if (current != null)
            {
                if (current.IsLeaf())
                {
                    Codes.Add(current.Char, code);
                }
                else
                {
                    GenerateCodes(current.Left, code + "0");
                    GenerateCodes(current.Right, code + "1");
                }
            }
        }

    }
}
