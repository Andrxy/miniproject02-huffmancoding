using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace miniproject02_huffmancoding.IO
{
    public class Decompressor
    {
        private Dictionary<string, char?> Codes;
        public Decompressor() 
        { 
            Codes = new Dictionary<string, char?>();
        }
        public string Decompress(byte[] data)
        {
            return LoadCodeTable(data);
        }

        private string LoadCodeTable(byte[] data)
        {
            int index = 0;
            int numCharacters = (int) data[index];
            index++;

            for (int i = 0; i < numCharacters; ++i)
            {
                char character = (char) data[index];
                index++;
                int numBits = (int) data[index];
                index++;
                int numBytes = (numBits + 7) / 8;

                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < numBytes; ++j)
                {
                    int code = (int) data[index];
                    int bitsToTake = Math.Min(numBits, 8);

                    for (int k = 7; k >= 8 - bitsToTake; --k)
                    {
                        sb.Append(((code >> k) & 1) == 1 ? '1' : '0');
                    }

                    numBits = numBits - bitsToTake;
                    index++;
                }

                // Console.WriteLine(sb.ToString());
                // Console.ReadKey();

                Codes[sb.ToString()] = character;
            }

            StringBuilder message = new StringBuilder();
            StringBuilder temp = new StringBuilder();
            for (int i = index; i < data.Length; ++i)
            {
                byte curByte = data[i];
                for (int j = 7; j >= 0; --j)
                {
                    bool isTurnedOn = ((curByte >> j) & 1) == 1;
                    temp.Append(isTurnedOn ? '1' : '0');

                    if (Codes.ContainsKey(temp.ToString()))
                    {
                        message.Append(Codes[temp.ToString()]);
                        temp = new StringBuilder();
                    }
                }
            }

            return message.ToString();
        }
    }
}
