using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.IO.Codecs.Subtitles;
using Vapp.Media;

namespace Vapp.Platform.Windows.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BufferHeight = 2000;

            RegisterSpecialCharacters();
            RegisterCommands();

            while (!done)
                Prompt();
        }

        static IEnumerable<string> SpecialCharacterList { get; set; }

        static Dictionary<string, Action<IEnumerable<string>>> CommandMapping { get; set; }

        static DirectoryInfo dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        static bool done = false;

        static void RegisterCommands()
        {
            CommandMapping = new Dictionary<string, Action<IEnumerable<string>>>();
            CommandMapping.Add("cd", ChangeDirectoryCommand);
            CommandMapping.Add("exit", ExitCommand);
            CommandMapping.Add("srt", SrtTestCommand);
        }

        static void RegisterSpecialCharacters()
        {
            List<string> special = new List<string>();
            special.Add("\\\"");
            SpecialCharacterList = special;
        }

        static void Prompt()
        {
            Console.Write(string.Format("{0}>", dir.ToString()));
            string input = Console.ReadLine().Trim();

            if (input != null && input.Length > 0)
            {
                string key = input.Split(' ').FirstOrDefault();
                IEnumerable<string> args = GetArguments(input);

                if (CommandMapping.ContainsKey(key.ToLower()))
                    CommandMapping[key].Invoke(GetArguments(input));
                else
                    ErrorWriteLine("Command does not exist");
            }
            else
                ErrorWriteLine("No input provided");
        }

        static string RemoveSpecialCharacters(string input)
        {
            string output = input;
            foreach (string character in SpecialCharacterList)
                output = output.Replace(character, character.Remove(0, 1));

            return output;
        }

        static IEnumerable<string> GetArguments(string input)
        {
            string key = input.Split(' ').FirstOrDefault();
            input = input.Remove(0, key.Length).Trim();

            List<string> args = new List<string>();
            int start = 0;
            bool block = false;
            bool reading = false;
            char p = ' ';

            for (int index = 0; index < input.Length; index++)
            {
                char c = input.ElementAt(index);

                if (c == ' ' && !block && reading)
                {
                    args.Add(input.Substring(start, index - start));
                    reading = false;
                }
                else if (c == '"' && p != '\\')
                {
                    if (block && reading)
                    {
                        args.Add(RemoveSpecialCharacters(input.Substring(start + 1, index - start - 1)));
                        block = reading = false;
                    }
                    else if (!block && !reading)
                    {
                        start = index;
                        block = reading = true;
                    }
                }
                else if (c != ' ' && !reading)
                {
                    start = index;
                    reading = true;
                }

                if (index == input.Length - 1 && reading)
                    args.Add(input.Substring(block ? start + 1 : start, block ? index - start - 1 : index - start + 1));

                p = c;
            }

            return args;
        }

        static void ErrorWriteLine(string line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void ChangeDirectoryCommand(IEnumerable<string> input)
        {
            if (input != null && input.Count() > 0)
            {
                try
                {
                    string path = input.ElementAt(0);
                    path = path.Last() == ':' ? path + "\\" : path;

                    DirectoryInfo dInfo = new DirectoryInfo(path);

                    if (dInfo.Exists)
                        dir = dInfo;
                    else
                        ErrorWriteLine("Directory does not exist");
                }
                catch
                {
                    ErrorWriteLine("Directory does not exist");
                }
            }
            else
                ErrorWriteLine("No argument provided");

        }

        static void ExitCommand(IEnumerable<string> input)
        {
            done = true;
        }

        static void SrtTestCommand(IEnumerable<string> input)
        {
            if (input.Count() != 1)
            {
                ErrorWriteLine("Invalid arguments provided");
                return;
            }

            Console.WriteLine("SRT TEST--------");
            string path = dir.FullName + "\\" + input.ElementAt(0);
            if (File.Exists(path))
            {
                IEnumerable<Subtitle> subtitles;
                Console.WriteLine("Opening stream");
                using (Stream stream = File.OpenRead(path))
                {
                    if (new SrtDecoder().TryDecode(stream, out subtitles))
                        Console.WriteLine("Stream decoded");
                    else
                    {
                        subtitles = null;
                        ErrorWriteLine("Could not decode stream");
                    }

                    Console.WriteLine("Closing stream");
                    stream.Close();
                }
                Console.WriteLine("Stream closed");

                if (subtitles != null)
                {
                    Console.WriteLine("VALUES----------");

                    foreach (Subtitle subtitle in subtitles)
                        Console.WriteLine(string.Format("Start: {0}, End: {1}, Text: {2}", subtitle.Start.ToString(@"hh\:mm\:ss\:fff"), subtitle.End.ToString(@"hh\:mm\:ss\:fff"), subtitle.Text));
                    
                    Console.WriteLine("----------------");
                }
            }
            else
            {
                ErrorWriteLine("File does not exist");
            }
            Console.WriteLine("----------------");
        }
    }
}
