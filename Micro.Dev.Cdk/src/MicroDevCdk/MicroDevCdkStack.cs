using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace MicroDevCdk
{
    public class MicroDevCdkStack : Stack
    {
        internal MicroDevCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            _ = new Function(this, "MicroDevFunction", new FunctionProps
            {
                Runtime = Runtime.PROVIDED_AL2023,
                Handler = "bootstrap",
                Code = Code.FromAsset(@"C:\Users\8enwi\OneDrive\Desktop\Micro.Dev.Lambda.zip"),
                Architecture = Architecture.ARM_64,
                MemorySize = 128,
                Timeout = Duration.Seconds(10),
                Description = ".NET 9 AOT Lambda on Graviton4",
            });
        }
    }
}
