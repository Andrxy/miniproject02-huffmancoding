using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace miniproject02_huffmancoding.IO
{
    internal class FileManager
    {
        private readonly string CompressedPath = @"..\..\Resources\Compressed";
        public void SaveBinaryFile(string fileName, byte[] data)
        {
            try
            {
                string finalPath = Path.Combine(CompressedPath, fileName + ".bin");

                using (FileStream fileStream = new FileStream(finalPath, FileMode.Create, FileAccess.Write))
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(data);
                }
            }
            catch (Exception e){
                throw new IOException("Error guardando archivo comprimido", e);
            }
        }

        public byte[] LoadBinaryFile(string fileName)
        {
            try
            {
                string finalPath = Path.Combine(CompressedPath, fileName + ".bin");

                byte[] data = File.ReadAllBytes(finalPath);

                return data;
            }
            catch (Exception e)
            {
                throw new IOException("Error cargando archivo comprimido", e);
            }
        }

        public string LoadTextFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"El archivo '{path}' no existe.");
            return File.ReadAllText(path);
        }

        public string[] GetCompressedFiles()
        {
            return Directory.GetFiles(CompressedPath, "*.bin");
        }
    }
}
