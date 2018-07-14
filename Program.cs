/***
 * Author: Bao Nguyen
 * 
 * Find 3 shortest paths using Dijstrak's algorithm.
 * The program reads a graph file that defines the visual 
 * of the graph with all vertices and connected edges with 
 * time and cost on each edge. Then it reads a search file 
 * that lists what paths to search for and what value (time vs cost)
 * to optimize for. It then uses modified Dijstrak's algorithm to find 
 * the shortest 3 paths (if possible). If there is any path that 
 * cannot be defined, it will display to the screen as "No path available". 
 * All other path will be displayed as:
 * "Start -> next -> ... -> end. Time: [time], Cost: [cost]"
 * The program will terminate safely if it cannot read
 * at least one file with an error indication to the screen.
 */


using System;
using System.Collections.Generic;
using System.IO;
using FlightGraph.Model;

namespace FlightGraph
{
    internal class Program
    {
        private enum File { GraphFile, SearchFile, ResultsFile };
        private enum Search { From, To, Comparator };

        private static void Main(string[] args)
        {

            string[] files = GetFiles();

            try
            {
                Graph flights = new Graph(files[(int)File.GraphFile]);
                var searches = GetSearchFlights(files[(int)File.SearchFile]);
                StreamWriter outputFile = new StreamWriter(files[(int)File.ResultsFile]);

                using (outputFile)
                {
                    // find shortest paths for every flight
                    for (var flight = 0; flight < searches.Count; flight++)
                    {
                        // setup data to find the shortest path(s)
                        string from = searches[flight][(int) Search.From],
                            to = searches[flight][(int) Search.To],
                            comparator = searches[flight][(int) Search.Comparator].Equals(Graph.COST) ? "COST" : "TIME",
                            flightInfo = $"FLIGHT {flight + 1}: from {from.ToUpper()} to {to.ToUpper()} (by {comparator})\r\n";

                        // display flight info
                        Console.Write(flightInfo);
                        outputFile.Write(flightInfo);

                        // find shortest paths
                        var results = flights.FindShortestPath(from, to, searches[flight][(int)Search.Comparator]);

                        // print paths to the screen and to the output file
                        for (var path = 0; path < results.Length; path++)
                        {
                            Console.Write($"Path {path + 1}: {results[path]}");
                            outputFile.Write($"Path {path + 1}: {results[path]}");
                        }

                        Console.WriteLine();
                        outputFile.WriteLine();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Cannot open at least 1 of these files: \n\t'{files[0]}'\n\t'{files[1]}'\n\t'{files[2]}");
            }
        }   
        

        private static List<string[]> GetSearchFlights(string fileName)
        {
            var searches = new List<string[]>();

            StreamReader iFile = new StreamReader(fileName);

            using (iFile)
            {
                int count = int.Parse(iFile.ReadLine() ?? throw new InvalidOperationException());

                for (int i = 0; i < count; i++)
                {
                    string input = iFile.ReadLine();
                    if (input != null)
                    {
                        string[] data = input.Split('|');
                        searches.Add(data);
                    }
                }
            }

            return searches;
        }

        
        private static string[] GetFiles()
        {
            var files = new string[3];

            Console.Write("Graph file name:  ");
            files[(int)File.GraphFile] = Console.ReadLine();
            Console.Write("Search file name: ");
            files[(int)File.SearchFile] = Console.ReadLine();
            Console.Write("Result file name: ");
            files[(int)File.ResultsFile] = Console.ReadLine();
            Console.WriteLine();

            return files;
        }
    }
}
