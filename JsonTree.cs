using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace FormaTest;
internal static class JsonTree
{
    internal static string? ValidateRequest(int count, int depth, string value)
    {
        var issue = (count, depth, value) switch
        {
            ( > 200, _, _) => "Maximum count is 200",
            ( < 1, _, _) => "Minimum count is 1",
            (_, > 40, _) => "Maximum depth is 40",
            (_, < 1, _) => "Minimum depth is 1",
            (int c, int d, _) when c < d => "Count can not be less than depth",
            (_, _, string s) when s.Length > 100 => "Maximum value length is 100",
            (_, _, _) => null,
        };
        return issue;
    }

    internal static JsonObject Build(int count, int depth, string value)
    {
        var node = new JsonObject { };
        int[] nodesPerLevel = RandomizeIntoBuckets(count, depth);
        var currentLevelNodes = new JsonObject[nodesPerLevel[0]];
        var allNodes = new JsonObject[depth][];
        allNodes[0] = currentLevelNodes;
        for (var i = 0; i < currentLevelNodes.Length; i++)
        {
            var newNode = new JsonObject();
            node.Add($"L1-N{i}", newNode);
            currentLevelNodes[i] = newNode;
        }
        for (var i = 1; i < depth; i++)
        {
            currentLevelNodes = AddLevel(currentLevelNodes, i + 1, nodesPerLevel[i]);
            allNodes[i] = currentLevelNodes;
        }
        InsertValueRandomly(allNodes, value);
        return node;
    }

    internal static JsonObject[] AddLevel(JsonObject[] nodes, int levelDisplayed, int nodeCountToAdd)
    {
        var newNodes = new JsonObject[nodeCountToAdd];
        int[] childCountPerNode = RandomizeIntoBuckets(nodeCountToAdd, nodes.Length);
        int nodeNum = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < childCountPerNode[i]; j++)
            {
                var newNode = new JsonObject();
                nodes[i].Add($"L{levelDisplayed}-N{nodeNum}", newNode);
                newNodes[nodeNum] = newNode;
                nodeNum++;
            }
        }
        return newNodes;
    }

    internal static int[] RandomizeIntoBuckets(int sum, int bucketCount)
    {
        if (bucketCount == 1)
        {
            return [sum];
        }

        if (bucketCount >= sum)
        {
            int[] selectedIndexes = Enumerable.Range(0, bucketCount).Shuffle().Take(sum).ToArray();
            int[] buckets = new int[bucketCount];
            foreach (var index in selectedIndexes)
            {
                buckets[index] = 1;
            }
            return buckets;
        }
        else
        {
            int[] cutPoints = Enumerable.Range(1, sum - 1).Shuffle().Take(bucketCount - 1).Order().ToArray();
            int[] buckets = new int[bucketCount];
            buckets[0] = cutPoints[0];
            buckets[^1] = sum - cutPoints[^1];
            for (var i = 1; i < bucketCount - 1; i++)
            {
                buckets[i] = cutPoints[i] - cutPoints[i - 1];
            }
            return buckets;
        }
    }

    internal static void InsertValueRandomly(JsonObject[][] allNodes, string value)
    {
        var outerTryOrder = Enumerable.Range(0, allNodes.Length).Shuffle();
        foreach (var i in outerTryOrder)
        {
            var innerTryOrder = Enumerable.Range(0, allNodes[i].Length).Shuffle();
            foreach (var j in innerTryOrder)
            {
                if (allNodes[i][j].Count == 0)
                {
                    allNodes[i][j]["value"] = value;
                    return;
                }
            }
        }
    }
}

[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(int))]
internal partial class Context : JsonSerializerContext { }

