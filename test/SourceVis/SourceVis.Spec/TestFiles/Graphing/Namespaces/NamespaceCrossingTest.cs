using Company.Project.ModuleA;
using Company.Project.ModuleB;
using Library;

namespace Company.Project.ModuleA
{
  file sealed partial class SharedService
  {
    public ExternalOrchestrator Ex = new();

    public void OperationA()
    {
      //empty test function
    }
  }
}

namespace Company.Project.ModuleB
{
  file sealed class Processor
  {
    public Processor()
    {
      new SharedService().OperationA();
      new SharedService().OperationB();
    }
  }
}

namespace Company.Project.ModuleA
{
  file sealed partial class SharedService
  {
    public Orchestrator _o = new();

    public void OperationB()
    {
      //empty test function
    }
  }
}

namespace Company.Project
{
  file sealed class Orchestrator
  {
    public Orchestrator()
    {
      _ = new Processor();
      new SharedService().OperationA();
      new SharedService().OperationB();
      _ = new ExternalOrchestrator();
    }
  }
}

namespace Library
{
  file sealed class ExternalOrchestrator
  {
    public ExternalOrchestrator()
    {
      new SharedService().OperationA();
      new SharedService().OperationB();
    }
  }
}