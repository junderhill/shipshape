using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Shipshape
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var filename = args[0];
            if (!File.Exists(filename))
                throw new ArgumentException($"File {filename} not found");

            using StreamReader reader = new StreamReader(filename, Encoding.UTF8);

            var yaml = new YamlStream();
            yaml.Load(reader);
            
            
            var rootNode = yaml.Documents[0].RootNode;
            
            var newRoot = new YamlMappingNode();
            
            foreach (var n in (YamlMappingNode) rootNode)
            {
                if(n.Key is YamlScalarNode {Value: "Resources"} scalarNode)
                {
                    var resources = n.Value as YamlMappingNode;
                    //
                    // var resourceNames = resources.Children.Select(x => (x.Key as YamlScalarNode).Value).OrderBy(s => s);
                    // foreach(var r in resourceNames)
                    //     Console.WriteLine(r);
                    
                    var children = new List<KeyValuePair<YamlNode, YamlNode>>();
                    foreach (var child in resources.Children.OrderBy(x => (x.Key as YamlScalarNode).Value))
                    {
                        children.Add(child); 
                    }
                    newRoot.Add(n.Key, new YamlMappingNode(children));
                }
                else
                {
                    newRoot.Add(n.Key, n.Value);
                }
            }

            var newDoc = new YamlDocument(newRoot);
            
            var newStream = new YamlStream(newDoc);
         
            newStream.Save(new StreamWriter("testoutput.yaml", false,Encoding.UTF8), true);
            
            // using StreamWriter writer = new StreamWriter(new FileStream("testoutput.yaml", FileMode.Create));
            // reader. 




        }
    }
   
    
}
