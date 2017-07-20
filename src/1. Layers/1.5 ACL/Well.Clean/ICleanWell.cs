using StructureMap;

namespace PH.Well.Clean
{
    public interface ICleanWell
    {
        void Process(IContainer container);
    }
}