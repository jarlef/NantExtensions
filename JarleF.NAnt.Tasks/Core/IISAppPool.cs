namespace JarleF.NAnt.Tasks.Core
{
    public class IISAppPool
    {
        public string Name { get; set; }
        public IISSiteStatus Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Status);
        }
    }
}
