using System;
using System.Collections.Generic;
using System.Text;

namespace miniproject02_huffmancoding.Utils
{
    internal class Compressor
    {
        public CompressionResult Compress(Dictionary<char?, string> codes, string input)
        {
            // 1. Convertir la tabla de códigos a bytes
            byte[] tableBytes = CodeTableToBytes(codes);

            // 2. Codificar el string de entrada a bits
            string bitString = EncodeInput(codes, input);

            // 3. Agregar padding para completar bytes
            string paddedBitString = PadBitString(bitString);

            // 4. Convertir string de bits a bytes
            byte[] contentBytes = ConvertToBytes(paddedBitString);

            // 5. Obtener tamaño del input
            byte[] inputSize = BitConverter.GetBytes(input.Length);

            // 6. Combinar tabla + tamaño + contenido
            byte[] compressedData = CombineTableAndContent(tableBytes, inputSize, contentBytes);


            int originalSize = Encoding.ASCII.GetByteCount(input);
            int compressedSize = compressedData.Length;

            Stats stats = new Stats();
            stats.OriginalSize = originalSize;
            stats.CompressedSize = compressedSize;
            stats.CompressionRatio = originalSize /  (double)compressedSize;
            stats.ReductionPercentage = 100.0 * (originalSize - compressedSize) / originalSize;

            CompressionResult result = new CompressionResult();
            result.Data = compressedData;
            result.Stats = stats;

            return result;
        }

        // Convierte la tabla de códigos a un arreglo de bytes
        private byte[] CodeTableToBytes(Dictionary<char?, string> codes)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte) codes.Count);

            foreach (var entry in codes)
            {
                char? character = entry.Key;
                string code = entry.Value;

                bytes.Add((byte) character);        // Carácter
                bytes.Add((byte) code.Length);      // # de bits
                bytes.AddRange(ConvertToBytes(code)); // Código en bytes
            }

            return bytes.ToArray();
        }

        // Convierte el input a un string de '0' y '1' según los códigos de cada carácter
        private string EncodeInput(Dictionary<char?, string> codes, string input)
        {
            StringBuilder encoded = new StringBuilder();
            foreach (char c in input)
                encoded.Append(codes[c]);
            return encoded.ToString();
        }

        // Agrega ceros al final para completar el último byte
        private string PadBitString(string bitString)
        {
            while (bitString.Length % 8 != 0)
                bitString += '0';
            return bitString;
        }
      
        //Convierte un string de bits a un arreglo de bytes
        private byte[] ConvertToBytes(string bitString)
        {
            List<byte> bytes = new List<byte>();
            bitString = PadBitString(bitString);

            for (int i = 0; i < bitString.Length; i += 8)
            {
                string substring = bitString.Substring(i, 8);
                bytes.Add(ToByte(substring));
            }

            return bytes.ToArray();
        }

        // Convierte 8 bits representados como string a un byte
        private byte ToByte(string bits, byte result = 0, int index = 0)
        {
            if (index == 8) return result;

            if (bits[index] == '1')
            {
                int shift = 7 - index;
                result = (byte) (result | (1 << shift));
            }

            return ToByte(bits, result, index + 1);
        }

        // Combina la tabla de códigos, tamaño del input y contenido codificado en un solo arreglo
        private byte[] CombineTableAndContent(byte[] tableBytes, byte[] inputSize, byte[] contentBytes)
        {
            byte[] result = new byte[tableBytes.Length + inputSize.Length + contentBytes.Length];
            tableBytes.CopyTo(result, 0);
            inputSize.CopyTo(result, tableBytes.Length);
            contentBytes.CopyTo(result, tableBytes.Length + inputSize.Length);
            return result;
        }
    }
}
