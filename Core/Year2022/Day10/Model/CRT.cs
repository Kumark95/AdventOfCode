namespace AdventOfCode.Core.Year2022.Day10.Model;

internal class CRT
{
    private const char LitPixel = '█';
    private const char DarkPixel = '░';
    private const int LineWidth = 40;

    private int CrtDrawingPosition { get; set; }
    private int SpriteMiddlePosition { get; set; }
    private List<int> SpritePositions => new() { SpriteMiddlePosition - 1, SpriteMiddlePosition, SpriteMiddlePosition + 1 };

    public List<char> Pixels { get; private set; }

    public CRT()
    {
        Pixels = new List<char>();
        CrtDrawingPosition = 0;
        SpriteMiddlePosition = 1; // 3-pixel wide. Center of the sprite 
    }

    public void MoveSprite(int offset)
    {
        SpriteMiddlePosition += offset;
    }

    public void DrawPixel()
    {
        if (SpritePositions.Contains(CrtDrawingPosition))
        {
            Pixels.Add(LitPixel);
        }
        else
        {
            Pixels.Add(DarkPixel);
        };

        CrtDrawingPosition = (CrtDrawingPosition + 1) % LineWidth;
    }

    public void DrawImage()
    {
        for (int i = 0; i < Pixels.Count; i++)
        {
            if (i > 0 && i % LineWidth == 0)
            {
                Console.WriteLine();
            }

            Console.Write(Pixels[i]);
        }
        Console.WriteLine();
    }
}
