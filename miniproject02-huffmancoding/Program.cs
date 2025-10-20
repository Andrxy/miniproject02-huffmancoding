using System;
using System.Collections.Generic;
using System.Reflection;
using miniproject02_huffmancoding.DataStructures;
using miniproject02_huffmancoding.IO;
using miniproject02_huffmancoding.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace HuffmanCompressor
{
    class Program
    {
        static void Main(string[] args)
        {
            FileManager fm = new FileManager();
            HuffmanTree tree = new HuffmanTree();
            Compressor compressor = new Compressor();
            Decompressor decompressor = new Decompressor();
            FrequencyCounter counter = new FrequencyCounter();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== COMPRESOR DE HUFFMAN ===");
                Console.WriteLine("1. Cargar archivo de texto");
                Console.WriteLine("2. Mostrar frecuencias");
                Console.WriteLine("3. Generar árbol y códigos");
                Console.WriteLine("4. Comprimir archivo");
                Console.WriteLine("5. Descomprimir archivo");
                Console.WriteLine("6. Salir");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();
                Console.Clear();

                switch (option)
                {
                    case "1":
                        Console.Write("Ruta del archivo: ");
                        string path = Console.ReadLine();

                        // string input = fm.LoadTextFile(path);

                        Console.WriteLine("\nArchivo cargado correctamente.\n");
                        // Console.WriteLine(input);
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Write("Ingrese texto para analizar: ");

                        string input = Console.ReadLine();
                        Dictionary<char?, int> freqs = counter.CountFrequencies(input);

                        Console.WriteLine("\nSímbolo\tFrecuencia");

                        foreach (var f in freqs)
                            Console.WriteLine($"{f.Key}\t{f.Value}");

                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Write("Texto: ");
                        string text = Console.ReadLine();

                        Dictionary<char?, int> freqTable = counter.CountFrequencies(text);
                        tree.BuildTree(freqTable);

                        Console.WriteLine("\nCódigos Huffman generados:\n");
                        foreach (var c in tree.Codes)
                            Console.WriteLine($"{c.Key}: {c.Value}");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Write("Texto a comprimir: ");
                        string textToCompress = Console.ReadLine();
                        var freqs2 = counter.CountFrequencies(textToCompress);
                        tree.BuildTree(freqs2);
                        byte[] data = compressor.Compress(tree.Codes, textToCompress);
                        fm.SaveBinaryFile("compressed", data);
                        Console.WriteLine("\nArchivo comprimido como 'compressed.bin'");
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.Write("Nombre del archivo comprimido (sin extensión): ");
                        string compressedName = Console.ReadLine();

                        byte[] compressedData = fm.LoadBinaryFile(compressedName);
                        string decompressedText = decompressor.Decompress(compressedData);

                        Console.WriteLine("\nArchivo descomprimido:\n");
                        Console.WriteLine(decompressedText);
                        Console.ReadKey();
                        break;

                    case "6":
                        return;
                }
            }
        }
    }
}
