using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using miniproject02_huffmancoding.Entities;

namespace miniproject02_huffmancoding.Utils
{
    public class FrequencyCounter
    {
        public Dictionary<char?, int> Frequencies { get; set; }

        public FrequencyCounter() { 
            this.Frequencies = new Dictionary<char?, int>();
        }

        public Dictionary<char?, int> CountFrequencies(string input)
        {
            foreach (char character in input)
            {
                if (!Frequencies.ContainsKey(character))
                    Frequencies.Add(character, 0);

                Frequencies[character]++;
            }

            return Frequencies;
        }
    }
}
