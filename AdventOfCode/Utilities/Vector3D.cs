namespace AdventOfCode.Utilities
{
    internal struct Vector3D
    {
        private long x;
        private long y;
        private long z;
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X
        {
            get => (int)x;
            set => x = value;
        }
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y
        {
            get => (int)y;
            set => y = value;
        }
        /// <summary>
        /// Z coordinate.
        /// </summary>
        public int Z
        {
            get => (int)z;
            set => z = value;
        }
        /// <summary>
        /// Point coordinates as an array;
        /// </summary>
        public int[] AsArray
        {
            get => new int[] { (int)x, (int)y, (int)z };
        }
        /// <summary>
        /// Point coordinates as an array;
        /// </summary>
        public long[] AsArrayLong
        {
            get => new long[] { x, y, z };
        }
        /// <summary>
        /// X coordinate as long.
        /// </summary>
        public long LongX
        {
            get => x;
            set => x = value;
        }
        /// <summary>
        /// Y coordinate as long.
        /// </summary>
        public long LongY
        {
            get => y;
            set => y = value;
        }
        /// <summary>
        /// Z coordinate as long.
        /// </summary>
        public long LongZ
        {
            get => z;
            set => z = value;
        }
        /// <summary>
        /// Vector magnitude squared.
        /// </summary>
        public int MagnitudeSquared
        {
            get => (int)(x * x + y * y + z * z);
        }
        /// <summary>
        /// Vector magnitude squared as long.
        /// </summary>
        public long MagnitudeSquaredLong
        {
            get => x * x + y * y + z * z;
        }

        public Vector3D(int x = 0, int y = 0, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3D(long x = 0, long y = 0, long z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public override string ToString() => $"({x}, {y}, {z})";
        public static Vector3D operator +(Vector3D p1, Vector3D p2) =>
            new Vector3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        public static Vector3D operator -(Vector3D p1, Vector3D p2) =>
            new Vector3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        public static Vector3D operator *(int m, Vector3D p) =>
            new Vector3D(m * p.X, m * p.Y, m * p.Z);
        public static Vector3D operator /(Vector3D p, int d) =>
            new Vector3D(p.X / d, p.Y / d, p.Z / d);
        public static bool operator ==(Vector3D p1, Vector3D p2) =>
            p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        public static bool operator !=(Vector3D p1, Vector3D p2) => !(p1 == p2);
    }
}
