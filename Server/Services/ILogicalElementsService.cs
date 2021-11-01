using Common;

namespace Server.Services
{
    public interface ILogicalElementsService
    {
        string AddElement(ElemType elemType);
        string SetValueForElement(string name, bool value);
        string AddIO(bool isInput, string name);
        string AddConnection(int idOfInput, int idOfOutput);
        string Show(int name);
        string Print();
    }
}