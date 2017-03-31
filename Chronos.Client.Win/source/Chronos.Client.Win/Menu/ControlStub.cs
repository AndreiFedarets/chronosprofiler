namespace Chronos.Client.Win.Menu
{
    public sealed class ControlStub : ControlCollection, IControlStub
    {
        private readonly string _id;

        public ControlStub(string id)
        {
            _id = id;
        }

        public override string Id
        {
            get { return _id; }
        }
    }
}
