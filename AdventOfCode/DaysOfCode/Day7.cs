
namespace AdventOfCode
{
    class Dir
    {
        public int Size
        {
            get
            {
                int size = 0;
                foreach (var file in Files)
                    size += file.Item2;
                foreach (var dir in Dirs)
                    size += dir.Size;

                return size;
            }
        }
        public List<Dir> Dirs { get; private set; }
        public List<Tuple<string, int>> Files { get; private set; }
        public Dir Predecessor { get; private set; }
        public string Name { get; set; }
        public Dir(string name, Dir predecessor)
        {
            Name = name;
            Dirs = new List<Dir>();
            Files = new List<Tuple<string, int>>();
            Predecessor = predecessor;
        }
        public void AddDirectory(Dir dir)
        {
            Dirs.Add(dir);
        }
        public void AddFile(string name, int size)
        {
            Files.Add(new Tuple<string, int>(name, size));
        }
    }
    internal class Day7
    {
        public static Dir GetDirTreeRoot(string console)
        {
            string[] lines = console.Split($"{Environment.NewLine}$ ").Select(x => x = "$ " + x).ToArray();

            Dir root = new Dir("/", null);
            foreach (string line in lines)
            {
                string[] commandAndOutput = line.Split(Environment.NewLine);
                string[] command = commandAndOutput[0].Split(' ');

                switch (command[1])
                {
                    case "cd":
                        string name = command[2];
                        if (name == "..")
                            root = root.Predecessor;
                        else
                            root = root.Dirs.Find(x => x.Name == name);
                        break;
                    case "ls":
                        string[] output = commandAndOutput.Skip(1).ToArray();
                        foreach (string outputLine in output)
                        {
                            string[] outputLineSplit = outputLine.Split(' ');
                            if (outputLineSplit[0] == "dir")
                                root.AddDirectory(new Dir(outputLineSplit[1], root));
                            else
                                root.AddFile(outputLineSplit[1], int.Parse(outputLineSplit[0]));
                        }
                        break;
                }
            }

            // Back to root
            while (root.Predecessor != null)
                root = root.Predecessor;

            return root;
        }
        public static int DirectoriesToDelete(string console, int maxSize)
        {
            int GetSumOfSuitableSizes(Dir root, int maxSize)
            {
                int sum = root.Size <= maxSize ? root.Size : 0;
                foreach (var dir in root.Dirs)
                    sum += GetSumOfSuitableSizes(dir, maxSize);
                return sum;
            }

            Dir root = GetDirTreeRoot(console);

            // Sum
            return GetSumOfSuitableSizes(root, maxSize);
        }
        public static int SmallestDirectoryToDelete(string console, int filesystemSize, int updateSize)
        {
            Dir root = GetDirTreeRoot(console);
            int spaceToFree = updateSize - filesystemSize + root.Size;

            int SmallestSize(Dir root)
            {
                int smallest = root.Size;
                foreach (var dir in root.Dirs)
                {
                    int size = SmallestSize(dir);
                    if (size >= spaceToFree)
                        smallest = Math.Min(smallest, size);
                }
                return smallest;
            }

            return SmallestSize(root);
        }
    }
}
