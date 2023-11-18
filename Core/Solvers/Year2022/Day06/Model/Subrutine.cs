namespace AdventOfCode.Core.Solvers.Year2022.Day06.Model;

internal class Subrutine
{
    internal static int FindStartOfPacket(string datastream, int markerLenght)
    {
        var index = 0;

        while (index < datastream.Length)
        {
            var candidateMarker = datastream.Substring(startIndex: index, length: markerLenght);
            var set = new HashSet<char>(candidateMarker.ToCharArray());
            if (set.Count == markerLenght)
            {
                break;
            }

            index++;
        }

        return index + markerLenght;
    }
}
