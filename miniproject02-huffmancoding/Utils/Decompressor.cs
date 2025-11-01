using System;
using System.Collections.Generic;
using System.Text;

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
            // 1. Leer la tabla de códigos Huffman y obtener índice de inicio y tamaño del contenido
            int inputSize;
            int index = ReadCodeTable(data, out inputSize);

            // 2. Decodificar el contenido
            return DecodeData(data, index, inputSize);
        }

        // Lee la tabla de códigos Huffman y devuelve el índice donde inicia el contenido
        private int ReadCodeTable(byte[] data, out int inputSize)
        {
            int index = 0; // Empezar desde el indice 0
            int numCharacters = data[index++];

            Codes.Clear(); // Eliminar Residuos

            for (int i = 0; i < numCharacters; i++)
            {
                char character = (char)data[index++];
                int numBits = data[index++];
                int numBytes = (numBits + 7) / 8;

                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < numBytes; j++)
                {
                    int codeByte = data[index++];
                    int bitsToTake = Math.Min(numBits, 8);

                    for (int k = 7; k >= 8 - bitsToTake; k--)
                        sb.Append(((codeByte >> k) & 1) == 1 ? '1' : '0');

                    numBits -= bitsToTake;
                }

                Codes[sb.ToString()] = character;
            }

            inputSize = BitConverter.ToInt32(data, index);
            index += 4;

            return index;
        }

        // Decodifica el contenido binario usando la tabla de códigos Huffman
        private string DecodeData(byte[] data, int startIndex, int inputSize)
        {
            StringBuilder message = new StringBuilder();
            StringBuilder temp = new StringBuilder();

            for (int i = startIndex; i < data.Length && inputSize > 0; i++)
            {
                byte curByte = data[i];
                for (int j = 7; j >= 0 && inputSize > 0; j--)
                {
                    bool isTurnedOn = ((curByte >> j) & 1) == 1;
                    temp.Append(isTurnedOn ? '1' : '0');

                    if (Codes.ContainsKey(temp.ToString()))
                    {
                        message.Append(Codes[temp.ToString()]);
                        temp.Clear();
                        inputSize--;
                    }
                }
            }

            return message.ToString();
        }
    }
}
