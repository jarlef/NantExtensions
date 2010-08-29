namespace JarleF.NAnt.Tasks.Core
{
    public class IISSite
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IISSiteStatus Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} ({2})", Id, Name, Status);
        }
    }
}
