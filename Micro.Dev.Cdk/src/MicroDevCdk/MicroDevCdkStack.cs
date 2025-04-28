using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace MicroDevCdk
{
    public class MicroDevCdkStack : Stack
    {
        internal MicroDevCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            _ = new Function(scope, "MicroDevFunction", new FunctionProps
            {

            });
        }
    }
}
