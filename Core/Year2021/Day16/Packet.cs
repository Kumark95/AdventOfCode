using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Year2021.Day16;

public class Packet
{
    private const int LiteralSegmentLength = 5;

    public int Version { get; set; }
    public PacketType Type { get; set; }
    public long Value { get; set; }
    public int ContentLength { get; set; }
    public int PacketLength { get { return ContentLength + 3 + 3; } } // 3 for version + 3 for id type
    public List<Packet> SubPackets { get; set; } = new List<Packet>();


    /// <summary>
    /// Sum the versions of the packet and each of its sub-packets
    /// </summary>
    /// <param name="packet"></param>
    /// <returns></returns>
    public static int SumVersions(Packet packet)
    {
        if (packet.Type == PacketType.Literal)
        {
            return packet.Version;
        }

        int sum = packet.Version;
        foreach (var subPacket in packet.SubPackets)
        {
            sum += SumVersions(subPacket);
        }

        return sum;
    }

    /// <summary>
    /// Decodes a binary string into a Packet
    /// </summary>
    /// <param name="binaryCode"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Packet Decode(string binaryCode)
    {
        // See: https://regex101.com/r/t9VajV/1
        var match = Regex.Match(binaryCode, @"^(?<Version>\d{3})(?<TypeId>\d{3})(?<PacketContent>.*)");
        if (!match.Success)
        {
            throw new Exception("Error parsing version and type");
        }

        var packet = new Packet
        {
            Version = Convert.ToInt32(match.Groups["Version"].Value, fromBase: 2),
            Type = (PacketType)Convert.ToInt32(match.Groups["TypeId"].Value, fromBase: 2)
        };

        var packetContent = match.Groups["PacketContent"].Value;
        if (packet.Type == PacketType.Literal)
        {
            var (value, contentLength) = DecodeLiteral(packetContent);

            packet.Value = value;
            packet.ContentLength = contentLength;
        }
        else
        {
            var (subPackets, contentLengt, value) = DecodeOperator(packetContent, packet.Type);
            packet.SubPackets = subPackets;
            packet.ContentLength = contentLengt;
            packet.Value = value;
        }

        return packet;
    }

    /// <summary>
    /// Decodes a literal packet
    /// </summary>
    /// <param name="binaryCode"></param>
    /// <returns></returns>
    private static (long value, int contentLength) DecodeLiteral(string binaryCode)
    {
        var literalContent = "";
        var idx = 0;
        while (idx < binaryCode.Length)
        {
            var segment = binaryCode.Substring(startIndex: idx, length: Math.Min(LiteralSegmentLength, binaryCode.Length - idx));
            literalContent += segment[1..];

            idx += LiteralSegmentLength;

            if (segment.StartsWith('0'))
            {
                break;
            }
        }

        return (value: Convert.ToInt64(literalContent, fromBase: 2), contentLength: binaryCode[..idx].Length);
    }

    /// <summary>
    /// Decodes an operator packet
    /// </summary>
    /// <param name="binartCode"></param>
    /// <param name="packetType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static (List<Packet> subPackets, int contentLength, long value) DecodeOperator(string binartCode, PacketType packetType)
    {
        var lengthType = (PacketLengthType)(binartCode[0] - '0');

        var subPackets = new List<Packet>();
        int contentLength;
        if (lengthType == PacketLengthType.TotalSubPacketLength)
        {
            // 15 bits represent the length
            var subPacketsLength = Convert.ToInt32(binartCode.Substring(startIndex: 1, length: 15), fromBase: 2);
            var subPacketsContent = binartCode.Substring(startIndex: 1 + 15, length: subPacketsLength);

            contentLength = 1 + 15 + subPacketsLength;

            // Decode the subPackets
            var workingContent = subPacketsContent;
            while (workingContent.Length > 0)
            {
                var subPacket = Decode(workingContent);

                workingContent = workingContent[subPacket.PacketLength..];

                subPackets.Add(subPacket);
            }
        }
        else
        {
            // 11 bits represent the length
            var numberOfSubPackets = Convert.ToInt32(binartCode.Substring(startIndex: 1, length: 11), fromBase: 2);
            var subPacketsContent = binartCode[(1 + 11)..];

            // Decode the subPackets
            var workingContent = subPacketsContent;
            contentLength = 1 + 11;
            while (subPackets.Count < numberOfSubPackets)
            {
                var subPacket = Decode(workingContent);

                workingContent = workingContent[subPacket.PacketLength..];

                subPackets.Add(subPacket);
                contentLength += subPacket.PacketLength;
            }
        }

        var subPacketsValues = subPackets.Select(s => s.Value).ToList();
        var value = packetType switch
        {
            PacketType.Sum => subPacketsValues.Sum(),
            PacketType.Product => subPacketsValues.Aggregate((long)1, (a, b) => a * b),
            PacketType.Minimum => subPacketsValues.Min(),
            PacketType.Maximum => subPacketsValues.Max(),
            PacketType.GreaterThan => Convert.ToInt64(subPacketsValues.First() > subPacketsValues.Last()),// Always contains 2 packets
            PacketType.LessThan => Convert.ToInt64(subPacketsValues.First() < subPacketsValues.Last()),// Always contains 2 packets
            PacketType.EqualTo => Convert.ToInt64(subPacketsValues.First() == subPacketsValues.Last()),// Always contains 2 packets
            _ => throw new Exception("Not valid operator type"),
        };
        return (subPackets, contentLength, value);
    }
}
