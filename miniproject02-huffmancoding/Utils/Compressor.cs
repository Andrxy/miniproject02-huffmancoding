using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace miniproject02_huffmancoding.IO
{
    internal class Compressor
    {
        public byte[] Compress(Dictionary<char?, string> codes, string input)
        {
            // 1. Convertir la tabla de códigos a bytes
            byte[] tableBytes = CodeTableToBytes(codes);

            // 3. Codificar el string de entrada a bits
            string bitString = EncodeInput(codes, input);

            // 4. Asegurarse de que la longitud sea múltiplo de 8
            string paddedBitString = PadBitString(bitString);

            // 5. Convertir string de bits a arreglo de bytes
            byte[] contentBytes = ConvertToBytes(paddedBitString);

            // 6. Combinar bytes de la tabla + bytes del contenido
            byte[] result = CombineTableAndContent(tableBytes, contentBytes);

            return result;
        }

            private byte[] CodeTableToBytes(Dictionary<char?, string> codes)
            {
                List<byte> bytes = new List<byte>();
                int size = codes.Count;
                bytes.Add((byte) size);

                foreach (char character in codes.Keys)
                {
                    string currentCode = codes[character];

                    bytes.Add((byte) character); // Carácter
                    bytes.Add((byte) currentCode.Length); // # de Bits
                    byte[] bitsToBytes = ConvertToBytes(currentCode);
                    bytes.AddRange(bitsToBytes);
                }

                return bytes.ToArray(); 
            }

        // Convierte el string original a un string de '0' y '1' según códigos
        private string EncodeInput(Dictionary<char?, string> codes, string input)
        {
            StringBuilder encoded = new StringBuilder();

            foreach (char character in input)
                encoded.Append(codes[character]);

            return encoded.ToString();
        }

        // Agrega ceros al final para completar el último byte
        private string PadBitString(string bitString)
        {
            while (bitString.Length % 8 != 0)
                bitString += '0';

            return bitString;
        }

        // Convierte un string de bits a un arreglo de bytes
        private byte[] ConvertToBytes(string paddedBitString)
        {
            List<byte> bytes = new List<byte>();

            paddedBitString = PadBitString(paddedBitString);

            for (int i = 0; i < paddedBitString.Length / 8; ++i)
            {
                string substring = paddedBitString.Substring(i * 8, 8);
                bytes.Add((byte) ToInt(substring));
            }

            return bytes.ToArray();
        }

        private byte[] CombineTableAndContent(byte[] tableBytes, byte[] contentBytes)
        {
            byte[] bytes = new byte[tableBytes.Length + contentBytes.Length];

            tableBytes.CopyTo(bytes, 0);
            contentBytes.CopyTo(bytes, tableBytes.Length);

            return bytes;
        }

        private int ToInt(string substring, int result = 0, int index = 0)
        {
            if (index == 8) return result;

            if (substring[index] == '1')
            {
                int shift = 7 - index;
                result = result | (1 << shift);
            }

            return ToInt(substring, result, index + 1);
        }
    }
}
