using System;
using System.Collections.Generic;
using System.IO;
using miniproject02_huffmancoding.DataStructures;
using miniproject02_huffmancoding.IO;
using miniproject02_huffmancoding.Utils;

namespace HuffmanCompressor
{
    class Program
    {
        static FileManager fm = new FileManager();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== COMPRESOR DE HUFFMAN ===");
                Console.WriteLine("1. Cargar archivo .txt y comprimir");
                Console.WriteLine("2. Escribir texto y comprimir");
                Console.WriteLine("3. Descomprimir archivo");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opción: ");
                string option = Console.ReadLine();
                Console.Clear();

                switch (option)
                {
                    case "1":
                        CompressFromFile();
                        break;
                    case "2":
                        CompressFromInput();
                        break;
                    case "3":
                        DecompressFile();
                        break;
                    case "4":
                        Console.WriteLine("Saliendo...");
                        return;
                    default:
                        Console.WriteLine("Opción inválida");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void CompressFromFile()
        {
            HuffmanTree tree = new HuffmanTree();
            Compressor compressor = new Compressor();
            FrequencyCounter counter = new FrequencyCounter();

            Console.Write("Ruta del archivo .txt: ");
            string path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("Archivo no encontrado.");
                Console.ReadKey();
                return;
            }

            string text = fm.LoadTextFile(path);
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("El archivo está vacío.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n--- Tabla de frecuencias ---");
            Dictionary<char?, int> freqs = counter.CountFrequencies(text);
            foreach (var entry in freqs)
                Console.WriteLine($"{entry.Key}\t{entry.Value}");

            tree.BuildTree(freqs);

            Console.WriteLine("\n--- Códigos Huffman ---");
            foreach (var code in tree.Codes)
                Console.WriteLine($"{code.Key}: {code.Value}");

            Console.Write("\nNombre del archivo comprimido (sin extensión): ");
            string binName = Console.ReadLine();

            byte[] compressed = compressor.Compress(tree.Codes, text);
            fm.SaveBinaryFile(binName, compressed);

            Console.WriteLine($"\nArchivo comprimido guardado en 'Resources/Compressed/{binName}.bin'");
            Console.ReadKey();
        }

        private static void CompressFromInput()
        {
            HuffmanTree tree = new HuffmanTree();
            Compressor compressor = new Compressor();
            FrequencyCounter counter = new FrequencyCounter();

            Console.Write("Escriba el texto a comprimir: ");
            string manualText = Console.ReadLine();

            Dictionary<char?, int> manualFreqs = counter.CountFrequencies(manualText);
            tree.BuildTree(manualFreqs);

            Console.WriteLine("\n--- Tabla de frecuencias ---");
            foreach (var f in manualFreqs)
                Console.WriteLine($"{f.Key}\t{f.Value}");

            Console.WriteLine("\n--- Códigos Huffman ---");
            foreach (var code in tree.Codes)
                Console.WriteLine($"{code.Key}: {code.Value}");

            Console.Write("\nNombre del archivo comprimido (sin extensión): ");
            string manualBin = Console.ReadLine();

            byte[] manualCompressed = compressor.Compress(tree.Codes, manualText);
            fm.SaveBinaryFile(manualBin, manualCompressed);

            Console.WriteLine($"\nArchivo comprimido guardado en 'Resources/Compressed/{manualBin}.bin'");
            Console.ReadKey();
        }

        private static void DecompressFile()
        {
            Decompressor decompressor = new Decompressor();

            Console.WriteLine("=== DESCOMPRESIÓN ===");
            string[] files = fm.GetCompressedFiles();

            if (files.Length == 0)
            {
                Console.WriteLine("No hay archivos en 'Resources/Compressed'.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Archivos disponibles:");
            for (int i = 0; i < files.Length; i++)
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");

            Console.Write("\nEscriba el nombre del archivo: ");
            string fileName = Console.ReadLine();

            try
            {
                byte[] compressedData = fm.LoadBinaryFile(fileName);
                string decompressedText = decompressor.Decompress(compressedData);

                Console.WriteLine("\n--- TEXTO DESCOMPRIMIDO ---");
                Console.WriteLine(decompressedText);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al descomprimir: {e.Message}");
            }

            Console.ReadKey();
        }
    }
}
