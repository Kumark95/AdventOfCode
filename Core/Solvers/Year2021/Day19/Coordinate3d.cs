using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2021.Day19;

internal record Coordinate3d : Point3d
{
    public Coordinate3d(int x, int y, int z) : base(x, y, z)
    {
    }

    public static Coordinate3d operator +(Coordinate3d a, Coordinate3d b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Coordinate3d operator -(Coordinate3d a, Coordinate3d b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    /// <summary>
    /// Change the orientation of the coordinate
    /// </summary>
    /// <param name="orientationOffset">Specific orientation to be applied</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Coordinate3d ChangeOrientation(int orientationOffset)
    {
        // All possible orientations in 3d.
        // Using a cube as an example, it can rotate in any of the 6 faces and for each
        // face rotate over the Z axis 4 times (0º, 180º, 90º, 270º)
        return orientationOffset switch
        {
            0 => this,
            1 => new Coordinate3d(X, -Z, Y),
            2 => new Coordinate3d(X, -Y, -Z),
            3 => new Coordinate3d(X, Z, -Y),
            4 => new Coordinate3d(-X, -Y, Z),
            5 => new Coordinate3d(-X, -Z, -Y),
            6 => new Coordinate3d(-X, Y, -Z),
            7 => new Coordinate3d(-X, Z, Y),
            8 => new Coordinate3d(Y, X, -Z),
            9 => new Coordinate3d(Y, -X, Z),
            10 => new Coordinate3d(Y, Z, X),
            11 => new Coordinate3d(Y, -Z, -X),
            12 => new Coordinate3d(-Y, X, Z),
            13 => new Coordinate3d(-Y, -X, -Z),
            14 => new Coordinate3d(-Y, -Z, X),
            15 => new Coordinate3d(-Y, Z, -X),
            16 => new Coordinate3d(Z, X, Y),
            17 => new Coordinate3d(Z, -X, -Y),
            18 => new Coordinate3d(Z, -Y, X),
            19 => new Coordinate3d(Z, Y, -X),
            20 => new Coordinate3d(-Z, X, -Y),
            21 => new Coordinate3d(-Z, -X, Y),
            22 => new Coordinate3d(-Z, Y, X),
            23 => new Coordinate3d(-Z, -Y, -X),
            _ => throw new Exception("Only rotations from 0..23 are allowed")
        };
    }
}
