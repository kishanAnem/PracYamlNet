using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.RepresentationModel;

namespace PracYamlNet
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo fileInfo = new FileInfo("/home/kishan/Documents/Projects/PracYamlNet/docker-compose.yml");

            YamlStream yamlStream = new YamlStream();

            yamlStream.Load(fileInfo.OpenText());

            var document = yamlStream.Documents[0];
            var node = (YamlMappingNode)document.RootNode;

            var items = ((YamlMappingNode)node.Children[new YamlScalarNode("services")]).Children;
            foreach (var child in items)
            {
                var key = GetScalarValue(child.Key);

                HandleService((YamlMappingNode)child.Value);
            }

        }

        public static string GetScalarValue(YamlNode node)
        {
            if (node.NodeType != YamlNodeType.Scalar)
            {
                throw new Exception();
            }

            return ((YamlScalarNode)node).Value!;
        }

        public static void HandleService(YamlMappingNode yamlMappingNode)
        {
            foreach (var child in yamlMappingNode!.Children)
            {
                var key = GetScalarValue(child.Key);

                switch (key)
                {
                    case "image":
                        Console.WriteLine("image {0}", GetScalarValue(child.Value));
                        break;
                    case "build":
                        HandleBuildMappings((YamlMappingNode)child.Value);
                        break;
                    case "ports":
                        HandlePortdMappings((YamlSequenceNode)child.Value);
                        break;
                }
            }
        }
        public static void HandleBuildMappings(YamlMappingNode yamlMappingNode)
        {
            foreach (var child in yamlMappingNode!.Children)
            {
                var key = GetScalarValue(child.Key);

                switch (key)
                {
                    case "context":
                        Console.WriteLine("context {0}", GetScalarValue(child.Value).ToLowerInvariant());
                        break;
                    case "dockerfile":
                        Console.WriteLine("dockerfile {0}", GetScalarValue(child.Value));
                        break;

                }
            }
        }

        public static void HandlePortdMappings(YamlSequenceNode yamlSequenceNode)
        {
            foreach (var child in yamlSequenceNode.Children)
            {
                var ports = GetScalarValue(child).Split(":");
                Console.WriteLine("host port {0}, container port {1}", ports[0], ports[1]);
            }
        }
    }

}


