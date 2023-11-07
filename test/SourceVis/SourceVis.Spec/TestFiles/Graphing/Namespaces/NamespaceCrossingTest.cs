

namespace Company.Project.ModuleA
{
    public partial class SharedService
    {
#pragma warning disable CS0169 // Field is never used
        private Library.ExternalOrchestrator _ex = new();
#pragma warning restore CS0169 // Field is never used
        public void OperationA()
        {
            //empty test function
        }
    }
}
namespace Company.Project.ModuleB
{
    public class Processor
    {
        public Processor()
        {
            new ModuleA
                    .SharedService()
                .OperationA();
            new ModuleA
                    .SharedService()
                .OperationB();
        }
    }
}
namespace Company.Project.ModuleA
{
    public partial class SharedService
    {
#pragma warning disable CS0169 // Field is never used
        private Orchestrator _o = new();
#pragma warning restore CS0169 // Field is never used
        public void OperationB()
        {
            //empty test function
        }
    }
}

namespace Company.Project
{
    using ModuleA;
    using ModuleB;

    public class Orchestrator
    {
        public Orchestrator()
        {
            _ = new Processor();
            new SharedService()
                .OperationA();
            new SharedService()
                .OperationB();
            _ = new Library
                .ExternalOrchestrator();
        }
    }
}

namespace Library
{
    public class ExternalOrchestrator
    {
        public ExternalOrchestrator()
        {
            new Company.Project
                    .ModuleA.SharedService()
                .OperationA();
            new Company.Project
                    .ModuleA.SharedService()
                .OperationB();
        }
    }
}